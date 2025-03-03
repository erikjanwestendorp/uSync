﻿using System.Collections.Generic;
using System.Linq;

using HtmlAgilityPack;

using Newtonsoft.Json;

using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;

using uSync.Core.Dependency;

namespace uSync.Core.Mapping
{
    public class MacroMapper : SyncValueMapperBase, ISyncMapper
    {
        private readonly IMacroService macroService;

        public override string Name => "Macro Mapper";

        public override string[] Editors => new string[] {
            $"{Constants.PropertyEditors.Aliases.TinyMce}.Macro",
            $"{Constants.PropertyEditors.Aliases.Grid}.Macro",
            $"{Constants.PropertyEditors.Aliases.Grid}.rte.Macro"
        };

        public MacroMapper(IEntityService entityService,
            IMacroService macroService)
            : base(entityService)
        {
            this.macroService = macroService;
        }

        public override IEnumerable<uSyncDependency> GetDependencies(object value, string editorAlias, DependencyFlags flags)
        {
            if (value == null) return Enumerable.Empty<uSyncDependency>();

            var attempt = value.TryConvertTo<string>();
            if (!attempt || string.IsNullOrWhiteSpace(attempt.Result)) return Enumerable.Empty<uSyncDependency>();

            var macro = LoadMacroValue(attempt.Result);
            if (macro == null) return Enumerable.Empty<uSyncDependency>();

            var macroItem = macroService.GetByAlias(macro.macroAlias);
            if (macroItem != null)
            {
                return new uSyncDependency()
                {
                    Flags = DependencyFlags.None,
                    Level = 1,
                    Name = macroItem.Name,
                    Order = DependencyOrders.Macros,
                    Udi = macroItem.GetUdi()
                }.AsEnumerableOfOne();
            }

            return Enumerable.Empty<uSyncDependency>();
        }


        private MacroValue LoadMacroValue(string macroString)
        {
            if (!macroString.DetectIsJson())
            {
                return LoadFromMarkup(macroString);
            }

            return JsonConvert.DeserializeObject<MacroValue>(macroString);
        }

        /// <summary>
        ///  Loads the Macro from a html tag
        /// </summary>
        private MacroValue LoadFromMarkup(string markup)
        {
            var attributes = GetMacroAttributes(markup);
            if (attributes.ContainsKey("macroAlias"))
            {
                var macroValue = new MacroValue()
                {
                    macroAlias = attributes["macroAlias"],
                    macroParamsDictionary = attributes
                        .Where(x => x.Key != "macroAlias")
                        .ToDictionary(k => k.Key, v => v.Value)
                };

                return macroValue;
            }

            return null;
        }

        /// <summary>
        ///  gets all the attributes from an html macro tag 
        /// </summary>
        private Dictionary<string, string> GetMacroAttributes(string html)
        {
            var doc = new HtmlDocument();
            doc.OptionOutputOriginalCase = true;
            doc.LoadHtml(html);

            return doc.DocumentNode.ChildNodes[0].Attributes
                .ToDictionary(k => k.OriginalName, v => v.Value);
        }

        private class MacroValue
        {
            public MacroValue()
            {
                macroParamsDictionary = new Dictionary<string, string>();
            }

            public string macroAlias { get; set; }
            public Dictionary<string, string> macroParamsDictionary { get; set; }
        }

    }
}

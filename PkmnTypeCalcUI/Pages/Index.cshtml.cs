using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using PokemonTypeLibrary.Models;

namespace PkmnTypeCalcUI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        // collections used by pokemon defending type comboboxes
        public List<IPkmnType> PrimaryPkmnTypeList { get; set; } = PkmnTypeFactory.GeneratePkmnTypeList();
        public List<IPkmnType> SecondaryPkmnTypeList { get; set; } = PkmnTypeFactory.GeneratePkmnTypeList();

        [BindProperty(SupportsGet = true)]
        public string SelectedPrimaryTypeName { get; set; }

        public ColorConverter cc { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SelectedSecondaryTypeName { get; set; }

        // collection used by objectlistview
        [BindProperty]
        public List<IPkmnType> PkmnTypeList { get; set; } = PkmnTypeFactory.GeneratePkmnTypeList();

        [BindProperty(SupportsGet = true)]
        public string SelectedPrimaryTypeColor { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SelectedSecondaryTypeColor { get; set; }

        IPkmnType selectedPrimaryType { get; set; }
        IPkmnType selectedSecondaryType { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;

            // add empty pkmn types to the comboboxes
            PrimaryPkmnTypeList.Insert(0, PkmnTypeFactory.CreateEmptyPkmnType());
            SecondaryPkmnTypeList.Insert(0, PkmnTypeFactory.CreateEmptyPkmnType());

        }

        public void OnGet()
        {
            // check for nulls and change to (none) if types are not selected
            if(SelectedPrimaryTypeName == null)
            {
                SelectedPrimaryTypeName = "(none)";
                SelectedPrimaryTypeColor = "white";
            }
            //if(SelectedPrimaryType == null)
            //{
            //    SelectedPrimaryType = PkmnTypeFactory.CreateEmptyPkmnType();
            //}
            //if(SelectedSecondaryType == null)
            //{
            //    SelectedSecondaryType = PkmnTypeFactory.CreateEmptyPkmnType();
            //}
            if (SelectedSecondaryTypeName == null)
            {
                SelectedSecondaryTypeName = "(none)";
                SelectedSecondaryTypeColor = "white";
            }

            

            selectedPrimaryType = PrimaryPkmnTypeList.Where(type => type.TypeName == SelectedPrimaryTypeName).FirstOrDefault();
            selectedSecondaryType = SecondaryPkmnTypeList.Where(type => type.TypeName == SelectedSecondaryTypeName).FirstOrDefault();

            SelectedPrimaryTypeColor = selectedPrimaryType.TypeColor;
            SelectedSecondaryTypeColor = selectedSecondaryType.TypeColor;

            // check if both comboboxes select the (none) type
            if (SelectedPrimaryTypeName == "(none)" && SelectedSecondaryTypeName == "(none)")
            {
                return;
            }

            // calculate damage multiplier for each pkmn type in the list
            foreach (var pkmnType in PkmnTypeList)
            {
                pkmnType.DmgMultiplier = pkmnType.CalculateDmgMultiplier(selectedPrimaryType, selectedSecondaryType);
            }

            // sort by damage multiplier from highest to lowest
            PkmnTypeList.Sort((x, y) => y.DmgMultiplier.CompareTo(x.DmgMultiplier));
        }
    }
}

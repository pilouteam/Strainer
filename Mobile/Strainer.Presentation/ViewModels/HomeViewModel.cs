using System;
using Cirrious.MvvmCross.ViewModels;
using Amp.MvvmCross;
using Amp.Extensions;
using System.Collections.Generic;
using Strainer.Common.Contracts;
using System.Windows.Input;
using System.Linq;

namespace Strainer.Presentation.ViewModels
{
    public class HomeViewModel : PageViewModel
    {
        private List<Cocktail> _cocktails;

        public HomeViewModel()
        {
            _cocktails = new List<Cocktail>()
            {
                new Cocktail(){ Name = "Mojito", Alcool = "Rum Blanc, Lime", Img = "mojito" },
                new Cocktail(){ Name = "Ti-Punch", Alcool = "Rhum Agricole, Lime", Img = "cock" },
                new Cocktail(){ Name = "Old Fashion", Alcool = "Bourbon, Bitters", Img = "old_fashion" },
                new Cocktail(){ Name = "Ti-Punch", Alcool = "Rhum Agricole, Lime", Img = "ti_punch" },
                new Cocktail(){ Name = "Old Fashion", Alcool = "Bourbon, Bitters", Img = "old_fashion" },
                new Cocktail(){ Name = "Ti-Punch", Alcool = "Rhum Agricole, Lime", Img = "ti_punch" },
            };

            Cocktails = new List<Cocktail>()
                {
                    new Cocktail(){ Name = "Mojito", Alcool = "Rum Blanc, Lime", Img = "mojito" },
                    new Cocktail(){ Name = "Ti-Punch", Alcool = "Rhum Agricole, Lime", Img = "cock" },
                    new Cocktail(){ Name = "Old Fashion", Alcool = "Bourbon, Bitters", Img = "old_fashion" },
                    new Cocktail(){ Name = "Ti-Punch", Alcool = "Rhum Agricole, Lime", Img = "ti_punch" },
                    new Cocktail(){ Name = "Old Fashion", Alcool = "Bourbon, Bitters", Img = "old_fashion" },
                    new Cocktail(){ Name = "Ti-Punch", Alcool = "Rhum Agricole, Lime", Img = "ti_punch" },
                };;
        }

        public override void OnViewStarted(bool firstTime)
        {
            base.OnViewStarted(firstTime);
            FirstTime = !firstTime;
        }

        public bool FirstTime{get;set;}

        public ICommand Refresh
        {
            get
            {
                return this.GetCommand(() =>
                    {
                        Cocktails = _cocktails;
                    });
            }
        }

        public ICommand DeleteCocktail
        {
            get
            {
                return this.GetCommand<Cocktail>( cocktail =>
                    {
                        var cocktails  = Cocktails;
                        cocktails.Remove(cocktail);
                        Cocktails = null;
                        Cocktails = cocktails;
                    });
            }
        }

        public ICommand ShareCocktail
        {
            get
            {
                return this.GetCommand<Cocktail>( cocktail  =>
                    {
                    });
            }
        }

        public List<Cocktail> Cocktails 
        { 
            get{ return GetValue<List<Cocktail>>();} 
            set{ SetValue(value);}
        } 

        public ICommand DeleteCocktails
        {
            get
            {
                return this.GetCommand<List<int>>(indexes =>
                    {
                        var cocktails = Cocktails;
                        var cocktailsToRemove = new List<Cocktail>();
                        foreach(var i in indexes)
                        {
                            cocktailsToRemove.Add(Cocktails.ElementAt(i));
                        }
                        Cocktails = null;
                        Cocktails = cocktails.Except(cocktailsToRemove).ToList();
                    });
            }
        }

        public ICommand ShareCocktails
        {
            get
            {
                return this.GetCommand<List<int>>(indexes =>
                    {
                        var cocktailsToShare = new List<Cocktail>();
                    });
            }
        }
    }
}


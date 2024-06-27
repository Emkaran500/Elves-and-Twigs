using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elves_and_Tweaks.Models
{
    public class Player : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private string? name;
        public string? Name
        {
            get => this.name;
            set
            {
                this.name = value;

                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                }
            }
        }
        private int mana;
        public int Mana
        {
            get => this.mana;
            set
            {
                this.mana = value;
                this.ManaString = "Mana: " + this.Mana.ToString();

                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Mana)));
                }
            }
        }
        public int tokens;
        public int Tokens
        {
            get => this.tokens;
            set
            {
                this.tokens = value;
                this.TokensString = "Tokens: " + this.Tokens.ToString();

                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Tokens)));
                }
            }
        }

        private string? manaString;
        public string? ManaString
        {
            get => this.manaString;
            set
            {
                this.manaString = value;

                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(ManaString)));
                }
            }
        }

        private string? tokensString;
        public string? TokensString
        {
            get => this.tokensString;
            set
            {
                this.tokensString = value;

                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(TokensString)));
                }
            }
        }

        public int modifiers;
        public int Modifiers
        {
            get => this.modifiers;
            set
            {
                this.modifiers = value;
                this.ModifiersString = "Modifiers: " + this.Modifiers.ToString();

                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(Modifiers)));
                }
            }
        }

        private string? modifiersString;
        public string? ModifiersString
        {
            get => this.modifiersString;
            set
            {
                this.modifiersString = value;

                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(ModifiersString)));
                }
            }
        }

        public Player(string? name, int mana, int tokens, int modifiers)
        {
            this.Name = name;
            this.Mana = mana;
            this.Tokens = tokens;
            this.Modifiers = modifiers;
        }
    }
}

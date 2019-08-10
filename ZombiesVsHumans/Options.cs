// License: GPLv3
// Author: Nuno Fachada

using System.Collections.Generic;

namespace ZombiesVsHumans
{
    public struct Options
    {
        public uint XDim { get; }
        public uint YDim { get; }
        public uint Zombies { get; }
        public uint Humans { get; }
        public uint PlayerZombies { get; }
        public uint PlayerHumans { get; }
        public uint Turns { get; }
        public bool Error { get; private set; }
        public string ErrorMessage { get; private set; }

        private static IList<string> validOptions;

        static Options()
        {
            validOptions = new List<string>()
                { "-x", "-y", "-z", "-h", "-Z", "-H", "-t" };
        }

        private Options(uint xDim, uint yDim, uint zombies, uint humans,
            uint playerZombies, uint playerHumans, uint turns)
        {
            XDim = xDim;
            YDim = yDim;
            Zombies = zombies;
            Humans = humans;
            PlayerZombies = playerZombies;
            PlayerHumans = playerHumans;
            Turns = turns;
            Error = false;
            ErrorMessage = null;
        }

        private Options(string error)
        {
            XDim = 0;
            YDim = 0;
            Zombies = 0;
            Humans = 0;
            PlayerZombies = 0;
            PlayerHumans = 0;
            Turns = 0;
            Error = true;
            ErrorMessage = error;
        }

        public static Options ParseArgs(string[] args)
        {

            Options optionsObject = new Options();
            Dictionary<string, uint> options = new Dictionary<string, uint>();

            if (args.Length != 2 * validOptions.Count)
            {
                return new Options("Invalid number of arguments");
            }

            for (int i = 0; i < args.Length; i += 2)
            {
                if (validOptions.Contains(args[i]))
                {
                    if (!options.ContainsKey(args[i]))
                    {
                        if (uint.TryParse(args[i + 1], out uint value))
                        {
                            options[args[i]] = value;
                        }
                        else
                        {
                            optionsObject = new Options(
                                $"Invalid value for option {args[i]}");
                            break;
                        }
                    }
                    else
                    {
                        optionsObject = new Options($"Repeated option: {args[i]}");
                        break;
                    }
                }
                else
                {
                    optionsObject = new Options($"Unknown option: {args[i]}");
                    break;
                }
            }

            if (!optionsObject.Error)
            {
                optionsObject = new Options(
                    options[validOptions[0]],
                    options[validOptions[1]],
                    options[validOptions[2]],
                    options[validOptions[3]],
                    options[validOptions[4]],
                    options[validOptions[5]],
                    options[validOptions[6]]);

                optionsObject.Validate();

            }

            return optionsObject;
        }

        public void Validate()
        {
            if (XDim == 0)
            {
                Error = true;
                ErrorMessage = "Horizontal dimension (x) must be > 0";
            }
            if (YDim == 0)
            {
                Error = true;
                ErrorMessage = "Vertical dimension (y) must be > 0";
            }
            if (Zombies < PlayerZombies)
            {
                Error = true;
                ErrorMessage =
                    "There are more player-controlled zombies than zombies";
            }
            if (Humans < PlayerHumans)
            {
                Error = true;
                ErrorMessage =
                    "There are more player-controller humans than humans";
            }
        }
    }
}
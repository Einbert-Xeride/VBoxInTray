using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VBoxInTray
{
    class GetOpt : IEnumerable<KeyValuePair<char, string>>
    {
        public class GetoptException : Exception
        {
            public GetoptException(string msg) : base(msg) { }
        }

        public class GetoptNeedArgException : GetoptException
        {
            public readonly char Opt;
            public GetoptNeedArgException(char opt) : base(string.Format("Option '{0}' needs an argument.", opt))
            {
                Opt = opt;
            }
        }

        public class GetoptUnknownOptException : GetoptException
        {
            public readonly char Opt;
            public GetoptUnknownOptException(char opt) : base(string.Format("Unknown option '{0}'", opt))
            {
                Opt = opt;
            }
        }

        private Dictionary<char, bool> optHasArg = new Dictionary<char, bool>();
        private string[] args;

        private GetOpt(string[] args, string shortopts)
        {
            this.args = args;
            for (int i = 0; i < shortopts.Length; ++i)
            {
                if (!char.IsLetterOrDigit(shortopts[i]) || (shortopts[i] > 127))
                {
                    throw new GetoptException(string.Format("Unexcepted character '{0}' in `shortopts`", shortopts[i]));
                }
                if (optHasArg.ContainsKey(shortopts[i]))
                {
                    throw new GetoptException(string.Format("Duplicate option '{0}' in `shortopts`", shortopts[i]));
                }
                bool hasarg = (i != shortopts.Length - 1 && shortopts[i + 1] == ':');
                optHasArg.Add(shortopts[i], hasarg);
                if (hasarg) ++i;
            }
        }

        /// <summary>
        /// <code>getopt</code>-like command line argument parser.
        /// </summary>
        /// <param name="args">command line arguments</param>
        /// <param name="shortopts">options, represented as the same as in C's <code>getopt</code>.</param>
        /// <returns>a key-value pair which its key represents option (or '\0') and its value represends argument (or null)</returns>
        public static IEnumerable<KeyValuePair<char, string>> Getopt(string[] args, string shortopts)
        {
            return new GetOpt(args, shortopts);
        }

        private IEnumerator<KeyValuePair<char, string>> yieldOptions()
        {
            bool alwaysArg = false;
            char opt = '\0'; // option that needs argument
            foreach (var arg in args)
            {
                if (arg[0] == '-' && !alwaysArg)
                {
                    if (arg == "--")
                    {
                        alwaysArg = true;
                        continue;
                    }
                    if (opt != '\0') throw new GetoptNeedArgException(opt);
                    for (int i = 1; i < arg.Length; ++i)
                    {
                        opt = arg[i];
                        if (optHasArg[opt])
                        {
                            if (i != arg.Length - 1) throw new GetoptNeedArgException(opt);
                            continue;
                        }
                        else
                        {
                            // opt temporarily not means "option that needs argument" here
                            yield return new KeyValuePair<char, string>(opt, null);
                            opt = '\0';
                        }
                    }
                }
                else
                {
                    yield return new KeyValuePair<char, string>(opt, arg);
                    opt = '\0';
                }
            }
            yield break;
        }

        public IEnumerator<KeyValuePair<char, string>> GetEnumerator()
        {
            return yieldOptions();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

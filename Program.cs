using System.Diagnostics;
using System.IO;
using System;
using System.Numerics;

namespace Sets {
    class Program {
        // static readonly int MAX_TESTCASE = 1000000;
        static readonly int N = 15;
        static readonly int MAX_N = 25;
        static readonly int I = 10;
        static readonly int MAX_I = 1000000;
        static TextWriter Log, File, Cons;

        static void Main(string[] args) {
            string logpath = "./log.txt";
            string outpath = "./out.txt";
            // string logpath = null;
            // string outpath = null;

            Cons = Console.Out;
            FileStream ostream0 = null;
            FileStream ostream1 = null;

            if(outpath != null) {
                try {
                    ostream0 = new FileStream(outpath, FileMode.Create, FileAccess.Write);
                    File = new StreamWriter(ostream0);
                    Console.SetOut(File);
                } catch(Exception e) {
                    Console.WriteLine("could not open '{0}'", outpath);
                    Console.WriteLine(e.Message);
                    return;
                }
            }

            if(logpath != null) {
                try {
                    ostream1 = new FileStream(logpath, FileMode.Create, FileAccess.Write);
                    Log = new StreamWriter(ostream1);
                } catch(Exception e) {
                    Console.WriteLine("could not open '{0}'", logpath);
                    Console.WriteLine(e.Message);
                    return;
                }
            }

            for(int i, n, v = 0; v < 2; ++v) {
                for(i = I; i <= MAX_I; i*=10)
                    RunTest(i, v == 0, "PrintSet", PrintSet);

                for(i = I; i <= MAX_I/20; i*=2)
                    RunTest(i, v == 0, "PrintPairs", PrintPairs);

                for(n = N; n <= MAX_N; ++n)
                    RunTest(n, v == 0, "PrintSubs", PrintSubs);
            }

            Cons.WriteLine("test complete");
            if(Log != null) Log.Close();
            if(Cons != null) Cons.Close();
            if(File != null) File.Close();
            if(ostream0 != null) ostream0.Close();
            if(ostream1 != null) ostream1.Close();
        }


        delegate void Action(int n, bool v);
        static void RunTest(int n, bool v, string name, Action testcase) {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            testcase(n, v);
            watch.Stop();

            foreach(var writer in new TextWriter[]{Cons, Log, File}) if(writer != null) {
                writer.WriteLine("------------------------------------------------");
                writer.WriteLine("{0}(n = {1}, verbose = {2}) complete", name, n, v);
                writer.WriteLine("Time passed: {0}", watch.Elapsed);
                writer.WriteLine("------------------------------------------------");
                writer.Flush();
            }
        }

        static void PrintSet(int n, bool v) {
            if(v) {
                for(int i = 1; i <= n ; ++i)
                    Console.Write("{0}{1}", i == 1 ? "" : ", ", i);

                Console.WriteLine();
            } else {
                for(int i = 1; i <= n ; ++i);
            }
        }

        static void PrintPairs(int n, bool v) {
            if(v) {
                for(int i = 1 ; i <= n ; ++i) {
                    for(int j = i ; j <= n  ; ++j)
                        Console.Write("({0}, {1}){2}", i, j, (j < n)?", " : "");
                        
                    Console.WriteLine();
                }
            } else {
                for(int i = 1 ; i <= n ; ++i)
                    for(int j = i ; j <= n  ; ++j);    
            }
        }

        static void PrintSubs(int n, bool v) {
            if(v) {
                for(int k = 0 ; k <= n ; ++k) {
                    BigInteger c  =  BinCoef(n, k);
                    Console.Write('{');

                    for(BigInteger l, w, i = 0; i < c; ++i) {
                        w = i/n;
                        l = i%n;

                        if(k == 0) Console.Write('{');
                        else Console.Write("{0}{1}", '{', l == 0 ? n : l);

                        for(int j = 1; j < k; ++j) {
                            int a = (int)((i + j + w) % n);
                            Console.Write(", {0}", a == 0 ? n : a);
                        }

                        Console.Write("{0}{1}", '}', i < c-1 ? ", " : "");
                    }

                    Console.WriteLine('}');
                }
            } else {
                for(int k = 0 ; k <= n ; ++k) {
                    BigInteger c  =  BinCoef(n,k);

                    for(BigInteger i = 0; i <= c; ++i) {
                        int w = (int)(i / n);
                        for(int j = 1 ; j < k ; ++j);
                    }
                }
            }
        }

        public static BigInteger Fac(int n) {
            BigInteger r = n == 0 ? 1 : n;
            while(n > 1) r *= (n -= 1);
            return r;
        }

        public static BigInteger BinCoef(int n, int k) {
            return Fac(n)/(Fac(n-k)*Fac(k));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using model;
using System.IO;
using System.Linq;
using System.Threading;

namespace Dom
{
    class main
    {
        const string f_source = "item.cs";
        static Store1 store1 = null;
        static Store2 cache2 = null;

        static void Main(string[] args)
        {
            new Thread(new ThreadStart(wsServer.Run)).Start();
            



            if (!File.Exists(f_source)) return;

            string src = File.ReadAllText(f_source);
            Type[] type = CompilerCode.Run(src);
            if (type != null && type.Length > 0)
            {
                {
                    switch (type[0].Name)
                    {
                        case "item1":
                            store1 = new Store1(type[0], src);
                            break;
                        case "item2":
                            cache2 = new Store2(type[0]);
                            break;
                    }
                    if (type.Length == 2)
                        cache2 = new Store2(type[1]);
                }

                if (store1.Count == 0)
                {
                    long key1 = store1.item_Add(new item1()
                    {
                        _status = 3,
                        _searchlevel = 1,
                        _xpathlevel = 5,
                        _urlid = 9999,
                        _id = "a",
                        _item = "a",
                        _name = "a",
                        _tag = "select",
                        _type = "select"
                    });

                    long key2 = store1.item_Add(new item1()
                    {
                        _status = 1,
                        _searchlevel = 1,
                        _xpathlevel = 9,
                        _urlid = 1111,
                        _id = "b",
                        _item = "b",
                        _name = "b",
                        _tag = "input",
                        _type = "text"
                    });
                }

                var rs1 = store1.item_Get(new oWhere() { Name = "_status", Opera = (byte)eOperator.EQUAL, Value = "3" });
                var rs2 = store1.item_Get(new oWhere() { Name = "_status", Opera = (byte)eOperator.EQUAL, Value = "9" });
                ;

                var a1 = store1.item_GetTop(10);

                long key3 = store1.item_Add(new item1()
                {
                    _status = 5,
                    _searchlevel = 5,
                    _xpathlevel = 6,
                    _urlid = 6666,
                    _id = "c",
                    _item = "c",
                    _name = "c",
                    _tag = "input",
                    _type = "check"
                });

                var a2 = store1.item_GetTop(10);

                var rs3 = store1.item_Get(new oWhere() { Name = "_status", Opera = (byte)eOperator.EQUAL, Value = "5" });
                var del = store1.item_GetByIndexLast();

                //store1.item_Remove(del);
                var all2 = store1.item_GetTop(10);

            }
            else
            {
            }

            Console.WriteLine("Finish ... ");
            Console.ReadKey();
        }



    }
}

﻿using BusterWood.Ducks;
using NUnit.Framework;
using System;

namespace UnitTests
{
    [TestFixture]
    public class StaticMethodTests
    {
        [Test]
        public void can_call_a_void_method_via_proxy()
        {
            ISimplist proxy = Duck.Cast<ISimplist>(typeof(TargetSimplist));
            proxy.Execute();
            Assert.AreEqual(1, TargetSimplist.calls);
        }

        [Test]
        public void can_call_a_void_method_with_parameter_via_proxy()
        {
            IWithNumber proxy = Duck.Cast<IWithNumber>(typeof(TargetWithParameter));
            proxy.Execute(2);
            Assert.AreEqual(2, TargetWithParameter.Number);
        }

        [Test]
        public void can_call_a_void_method_with_parameter_that_returns_something()
        {
            var proxy = Duck.Cast<IAdder>(typeof(Adder));
            Assert.AreEqual(3, proxy.AddOne(2));
        }

        [Test]
        public void can_proxy_system_io_file()
        {
            var proxy = Duck.Cast<IExister>(typeof(System.IO.File));
            Assert.AreEqual(true, proxy.Exists(@"c:\Windows\notepad.exe"));
        }

        [Test]
        public void proxy_inherited_interface_types()
        {
            var proxy = Duck.Cast<IExistDeleter>(typeof(System.IO.File));
            Assert.AreEqual(true, proxy.Exists(@"c:\Windows\notepad.exe"));
        }

        [Test]
        public void cannot_cast_if_a_target_method_is_missing()
        {
            Assert.Throws<InvalidCastException>(() => Duck.Cast<ISimplist>(typeof(TargetBad)));
        }

        public interface IExistDeleter : IExister, IDeleter
        {
        }
        public interface IExister
        {
            bool Exists(string path);
        }

        public interface IDeleter
        {
            void Delete(string path);
        }

        public interface ISimplist
        {
            void Execute();
        }

        public interface IWithNumber
        {
            void Execute(int num);
        }

        public interface IAdder
        {
            int AddOne(int num);
        }

        public class TargetSimplist
        {
            public static int calls;

            public static void Execute()
            {
                calls++;
            }
        }

        public class TargetWithParameter
        {
            public static int Number;

            public static void Execute(int num)
            {
                Number = num;
            }
        }

        public class Adder
        {
            public static int AddOne(int num) => num + 1;
        }

        public class TargetBad
        {
            public static void Fred2()
            {
            }
        }
    }
}
using System;
using System.Runtime.InteropServices;
using InterfaceUnsafe;

unsafe
{
    Foo foo = new();
    var handle = GCHandle.Alloc(foo, GCHandleType.Normal);
    var pointer = (int*)GCHandle.ToIntPtr(handle).ToPointer();
    Console.WriteLine($"0x{(long)pointer}");
    Console.WriteLine($"0x{(long)*pointer}");

    var aref = (int**)pointer;
    var vtable = (long***)pointer;

    for (var i = 0; i < 10; i++)
        Console.WriteLine("a ptr : " + *(*aref + i));
    for (var i = 0; i < 20; i++)
        Console.WriteLine($"vtable[{i}] : {*(**vtable + i)}");

    IBar ifoo = foo;
    var handle2 = GCHandle.Alloc(ifoo, GCHandleType.Normal);
    var pointer2 = (int*)GCHandle.ToIntPtr(handle2).ToPointer();
    var pointer3 = (long*)GCHandle.ToIntPtr(handle2).ToPointer() + 1;
    var pointer4 = (long*)GCHandle.ToIntPtr(handle2).ToPointer() + 2;
    Console.WriteLine($"0x{(long)pointer2}");
    Console.WriteLine($"0x{(long)*pointer2}");

    aref = (int**)pointer2;
    vtable = (long***)pointer2;

    for (var i = 0; i < 10; i++)
        Console.WriteLine("a ptr : " + *(*aref + i));
    for (var i = 0; i < 20; i++)
        Console.WriteLine($"vtable[{i}] : {*(**vtable + i)}");

    Console.WriteLine($"0x{(long)pointer3}");
    Console.WriteLine($"0x{(long)*pointer3}");
    var aref2 = (long**)pointer3;
    vtable = (long***)pointer3;

    for (var i = 0; i < 5; i++)
        Console.WriteLine("aa ptr : " + *(*aref2 + i));
    for (var i = 0; i < 20; i++)
        Console.WriteLine($"a vtable[{i}] : {*(**vtable + i)}");

    aref2 = (long**)pointer4;
    vtable = (long***)pointer4;

    for (var i = 0; i < 5; i++)
        Console.WriteLine("aa ptr : " + *(*aref2 + i));
    for (var i = 0; i < 20; i++)
        Console.WriteLine($"a vtable[{i}] : {*(**vtable + i)}");

    handle.Free();
    handle2.Free();

    //TestM(foo);
    //TestM2(foo);
    //TestM3(foo);
}

static unsafe void TestM(IBar iBar)
{
    var handle = GCHandle.Alloc(iBar, GCHandleType.Normal);
    var pointer = (int*)GCHandle.ToIntPtr(handle).ToPointer();
    Console.WriteLine($"0x{(long)pointer}");
    Console.WriteLine($"0x{(long)*pointer}");

    var aref = (int**)pointer;
    var vtable = (long***)pointer;

    for (var i = 0; i < 10; i++)
        Console.WriteLine("a ptr : " + *(*aref + i));
    for (var i = 0; i < 6; i++)
        Console.WriteLine($"vtable[{i}] : {*(**vtable + i)}");
    handle.Free();
}

static unsafe void TestM2(object o)
{
    var handle = GCHandle.Alloc(o, GCHandleType.Normal);
    var pointer = (int*)GCHandle.ToIntPtr(handle).ToPointer();
    Console.WriteLine($"0x{(long)pointer}");
    Console.WriteLine($"0x{(long)*pointer}");

    var aref = (int**)pointer;
    var vtable = (long***)pointer;

    for (var i = 0; i < 10; i++)
        Console.WriteLine("a ptr : " + *(*aref + i));
    for (var i = 0; i < 6; i++)
        Console.WriteLine($"vtable[{i}] : {*(**vtable + i)}");
    handle.Free();
}

static unsafe void TestM3(IComparable o)
{
    var handle = GCHandle.Alloc(o, GCHandleType.Normal);
    var pointer = (int*)GCHandle.ToIntPtr(handle).ToPointer();
    Console.WriteLine($"0x{(long)pointer}");
    Console.WriteLine($"0x{(long)*pointer}");

    var aref = (int**)pointer;
    var vtable = (long***)pointer;

    for (var i = 0; i < 10; i++)
        Console.WriteLine("a ptr : " + *(*aref + i));
    for (var i = 0; i < 6; i++)
        Console.WriteLine($"vtable[{i}] : {*(**vtable + i)}");
    handle.Free();
}

static IBar AsIBar(Foo foo) => foo as IBar;

namespace InterfaceUnsafe
{
    public class BaseFoo : IComparable
    {
        private int _g = 1123;
        public virtual void Bv() => throw new NotImplementedException();
        public int CompareTo(object obj) => throw new NotImplementedException();
    }

    public class Foo : BaseFoo, IBar, IBaz
    {
        private int _a = 10;
        private int _b = 1320;
        private int _c = 1330;
        private int _d = 1540;
        private int _e = 1640;
        private int _f = 1720;
        public override void Bv() => throw new NotImplementedException();
        public void Baz() => _a = 1;
        public void Hee() => throw new NotImplementedException();
        public void Qux() => _a = 2;
    }

    public interface IBar
    {
        void Baz();
        void Qux();
    }

    public interface IBaz
    {
        void Hee();
    }
}
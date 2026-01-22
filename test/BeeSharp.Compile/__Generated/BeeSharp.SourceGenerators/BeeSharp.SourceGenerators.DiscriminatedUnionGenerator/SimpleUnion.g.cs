#nullable enable

using System;

namespace BeeSharp.Compile.Test.Union;

partial class SimpleUnion
{
    private BeeSharp.Tests.Unions.Types.UnionClassType? A { get; }
    private BeeSharp.Tests.Unions.Types.UnionStructType? B { get; }
    private int? C { get; }
    
    public SimpleUnion(BeeSharp.Tests.Unions.Types.UnionClassType a)
    {
        ArgumentNullException.ThrowIfNull(a);
        this.A = a;
    }
    
    public SimpleUnion(BeeSharp.Tests.Unions.Types.UnionStructType b)
    {
        this.B = b;
    }
    
    public SimpleUnion(int c)
    {
        this.C = c;
    }
    
    
    public T Map<T>(
        Func<BeeSharp.Tests.Unions.Types.UnionClassType, T> mapA,
        Func<BeeSharp.Tests.Unions.Types.UnionStructType, T> mapB,
        Func<int, T> mapC)
    {
        if (this.A is not null) { return mapA(this.A); }
        if (this.B is not null) { return mapB(this.B.Value); }
        if (this.C is not null) { return mapC(this.C.Value); }
        return default!; // never reached
    }
    
    public BeeSharp.Tests.Unions.Types.UnionClassType UnwrapOrThrowA()
    {
        if (this.A is null) { throw new InvalidOperationException(); }
        return this.A;
    }
    
    public BeeSharp.Tests.Unions.Types.UnionStructType UnwrapOrThrowB()
    {
        if (this.B is null) { throw new InvalidOperationException(); }
        return this.B.Value;
    }
    
    public int UnwrapOrThrowC()
    {
        if (this.C is null) { throw new InvalidOperationException(); }
        return this.C.Value;
    }
    
    
    
    public override string ToString()
    {
        if(this.A is not null) { return $"U{{{ this.A }}}"; }
        if(this.B is not null) { return $"U{{{ this.B }}}"; }
        if(this.C is not null) { return $"U{{{ this.C }}}"; }
        return default!; // never reached
    }

}

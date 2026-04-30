using System;

public interface IStat_Provider
{
    double Current { get;}
    double Max { get; }

    event Action<double, double> On_Value_Changed;
}

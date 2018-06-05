using System;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.Devices;

/// <summary>
/// Summary description for My
/// </summary>
public class My
{
    static private Computer computer = new Computer();
    public My()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    static public Computer Computer
    {
        get { return computer; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Machine object
/// </summary>
public class Machine
{
    private string name = "";
    private string netbiosname = "";
    private string macaddress;
    private bool pinging;
    private int lastpingresult;

    private void internalping()
    {
        string host;
        // hostname even kopieren met een 'lock'
        lock (netbiosname) { host = netbiosname; }

        if (host.Length > 0)
        {
            try
            {
                int timeout = Environment.TickCount;
                if (My.Computer.Network.Ping(host))
                {
                    lastpingresult = (Environment.TickCount - timeout);
                }
                else
                {
                    lastpingresult = 0;
                }
            }
            catch
            {
                lastpingresult = 0;
            }
        }
        else
        {
            lastpingresult = 0;
        }
        pinging = false;
    }

    public Machine()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public Machine(string Netbios, string MacAddress)
    {
        netbiosname = Netbios;
        macaddress = MacAddress;
        PingASync();
    }

    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public string Netbios
    {
        get { return netbiosname; }
        set 
        {
            if (!netbiosname.Equals(value, StringComparison.CurrentCultureIgnoreCase))
            {
                lock (netbiosname)
                {
                    netbiosname = value;
                }
                lastpingresult = 0; 
            }
        }
    }

    public string MAC
    {
        get { return macaddress; }
        set { macaddress = value.ToUpper(); }
    }

    public bool Pinging
    {
        get { return pinging; }
    }

    public long LastPingResult
    {
        get { return lastpingresult; }
    }

    public bool Online
    {
        get { return lastpingresult > 0; }
    }

    public string ActionText
    {
        get
        {
            if (Online)
            {
                return "Afsluiten";
            }
            else
            {
                return "Opstarten";
            }

        }
    }

    /// <summary>
    ///     Voert in PING opdracht uit naar de aangegeven Host PC
    /// </summary>
    /// <returns>
    ///     Tijd in miliseconden, van de ping (indien '0', dan is ping niet gelukt)
    /// </returns>
    public long Ping()
    {
        if (netbiosname.Length > 0)
        {
            internalping();
        }
        return lastpingresult;
    }


    /// <summary>
    ///     Voert in PING opdracht asynchroon uit naar de aangegeven Host PC
    ///     Via het event kan het resultaat worden verwerkt
    /// </summary>
    public void PingASync()
    {
        if (netbiosname.Length > 0)
        {
            System.Threading.Thread pingthread = new System.Threading.Thread(
                    new System.Threading.ThreadStart(internalping));
            pinging = true;
            pingthread.Start();
        }
    }
}

public class MachineList
{
    public MachineList()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public Machine[] getMachines()
    {
        // Mogelijk machines uitlezen uit XML-bestand
        System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(typeof(Machine[]));
        System.IO.FileStream f = new System.IO.FileStream(HttpContext.Current.Server.MapPath(@"~\App_Data\MachineList.xml"), System.IO.FileMode.Open, System.IO.FileAccess.Read);
        Machine[] list = (Machine[])x.Deserialize(f);
        f.Close();

        // Start Pinging
        foreach (Machine c in list) { c.PingASync(); }

        // Wait for pings to finish
        foreach (Machine c in list)
        {
            while(c.Pinging) { System.Threading.Thread.Sleep(10); }
        }

        return list;
    }
}

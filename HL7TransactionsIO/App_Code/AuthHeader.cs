using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Protocols;

/// <summary>
/// Summary description for AuthHeader
/// </summary>
public class AuthHeader:SoapHeader
{
    public string UserName;
    public string Password;
    //public string userId;
    //public byte[] passwordDigest;
    //public string nonce;
    //public DateTime ts;
    //public byte[] message;
}
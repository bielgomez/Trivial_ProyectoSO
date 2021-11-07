using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.IO;
using System.Net;
using System.Net.Sockets;
namespace Trivial
{
    public class Consultas
		{
			

			public string codigo(string mensaje, Socket server)
			{
				string respuesta;
				byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
				server.Send(msg);

				//Recibimos la respuesta del servidor
				byte[] msg2 = new byte[80];
				server.Receive(msg2);
				respuesta = Encoding.ASCII.GetString(msg2).Split('\0')[0];
				return respuesta;
			}
		}
	
}

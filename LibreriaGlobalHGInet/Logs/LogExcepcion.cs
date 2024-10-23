using LibreriaGlobalHGInet.Funciones;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LibreriaGlobalHGInet.General
{
	public static class LogExcepcion
	{
		public static void Guardar(Exception ex)
		{
			try
			{
				var line = Environment.NewLine;

				string linea = string.Empty;

				string[] txt_tmp = null;

				if (ex.StackTrace != null)
					txt_tmp = ex.StackTrace.ToLowerInvariant().Split(new string[] { "línea", "line" }, StringSplitOptions.RemoveEmptyEntries);

				if (txt_tmp != null && txt_tmp.Count() >= 2)
					linea = txt_tmp[txt_tmp.Count() - 1].ToString().Trim();

				string mensaje = ex.GetType().Name.ToString();
				string tipo = ex.GetType().ToString();
				string data = ex.Message.ToString();
				string source = (ex.Source != null) ?  ex.Source.ToString() : "";


				string stack = string.Empty;

				if (ex.StackTrace != null)
					stack = ex.ToLogString(Environment.StackTrace);

				string filepath = string.Format(@"{0}\logs\", Directorio.ObtenerDirectorioRaiz());

				if (!Directory.Exists(filepath))
				{
					Directory.CreateDirectory(filepath);

				}
				filepath = filepath + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";

				if (!File.Exists(filepath))
				{
					File.Create(filepath).Dispose();
				}

				using (StreamWriter sw = File.AppendText(filepath))
				{
					string error = "Fecha:" + " " + DateTime.Now.ToString(Fecha.formato_fecha_hora_completa) + line
					+ "Línea #" + " " + linea + line
					+ "Mensaje:" + " " + mensaje + line
					+ "Tipo:" + " " + tipo + line
					+ "Descripción:" + " " + data + line
					+ "Stack:" + " " + stack + line
					+ "source:" + " " + source + line;

					if (ex.InnerException != null)
					{
						error += line + "Excepción Adjunta: " + ex.InnerException.Message + line;

						if (ex.InnerException.StackTrace != null)
							error += "Stack Adjunto: " + ex.InnerException.ToLogString(Environment.StackTrace) + line;
					}


					sw.WriteLine("<--->");
					sw.WriteLine(error);
					sw.Flush();
					sw.Close();

				}

			}
			catch (Exception e)
			{
				e.ToString();

			}
		}

		/// <summary>
		///  Provides full stack trace for the exception that occurred.
		/// </summary>
		/// <param name="exception">Exception object.</param>
		/// <param name="environmentStackTrace">Environment stack trace, for pulling additional stack frames.</param>
		public static string ToLogString(this Exception exception, string environmentStackTrace)
		{
			List<string> environmentStackTraceLines = LogExcepcion.GetUserStackTraceLines(environmentStackTrace);

			if (environmentStackTraceLines.Count() > 1)
				environmentStackTraceLines.RemoveAt(0);

			List<string> stackTraceLines = LogExcepcion.GetStackTraceLines(exception.StackTrace);
			stackTraceLines.AddRange(environmentStackTraceLines);

			string fullStackTrace = String.Join(Environment.NewLine, stackTraceLines);

			string logMessage = exception.Message + Environment.NewLine + fullStackTrace;
			return logMessage;
		}

		/// <summary>
		///  Gets a list of stack frame lines, as strings.
		/// </summary>
		/// <param name="stackTrace">Stack trace string.</param>
		private static List<string> GetStackTraceLines(string stackTrace)
		{
			return stackTrace.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();
		}

		/// <summary>
		///  Gets a list of stack frame lines, as strings, only including those for which line number is known.
		/// </summary>
		/// <param name="fullStackTrace">Full stack trace, including external code.</param>
		private static List<string> GetUserStackTraceLines(string fullStackTrace)
		{
			List<string> outputList = new List<string>();
			Regex regex = new Regex(@"([^\)]*\)) in (.*):line (\d)*$");

			List<string> stackTraceLines = LogExcepcion.GetStackTraceLines(fullStackTrace);
			foreach (string stackTraceLine in stackTraceLines)
			{
				if (!regex.IsMatch(stackTraceLine))
				{
					continue;
				}

				outputList.Add(stackTraceLine);
			}

			return outputList;
		}
	}
}

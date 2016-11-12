using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

using Microsoft.Research.Uncertain;
using Microsoft.Research.Uncertain.Inference;

namespace DependenceAnalysis
{
	public class Correlation
	{
		
		public List<double> temperature;
		public List<double> humidity;

		public UList<Tuple<Uncertain<double>, Uncertain<double>>> pairs;

		public UList<double> temperature_distribution;
		public UList<double> humidity_distribution;

		Gaussian noise;

		public Correlation ()
		{
			temperature = new List<double> ();
			humidity = new List<double> ();

			temperature_distribution = new UList<double> ();
			humidity_distribution = new UList<double> ();

			pairs = new UList<Tuple<Uncertain<double>, Uncertain<double>>> ();

			noise = new Gaussian (5, 1);
		}

		public void Parser(List<double> temperature, List<double> humidity) {

			StreamReader file = new StreamReader("temp_hum1.txt");
			string line;

			while ((line = file.ReadLine())!= null) {			
				double temp = Convert.ToDouble(line.Split(null).ElementAt(0).Trim());
				double hum = Convert.ToDouble (line.Split(null).ElementAt(1).Trim());
				temperature.Add (temp);
				humidity.Add (hum);
			}
		}

		// randomly pick some indices and add the same noise to the temperature and humidity in that index.
		public void AddChannelNoise(){
		
			// how many data to modify? maximum is the number of data we have from the sensor.
			int number_of_data_to_modify = new Random ().Next(0, temperature.Count);

			// for each of them, randomly pick an index and add the noise.
			for (int x=0; x<number_of_data_to_modify; x++) {
				Random rand = new Random ();
				int index = rand.Next (0, temperature.Count);

				// adding noise makes the temperature have a Gaussian distribution.
				var g_noisy_t = new Gaussian (temperature.ElementAt(index) , 1);
				temperature_distribution.Add (g_noisy_t);
			
				// adding noise makes the humidity have a Gaussian distribution.
				var g_noisy_h = new Gaussian (humidity.ElementAt(index) + 5, 1);
				humidity_distribution.Add (g_noisy_h);

			}		

			for (int x=0; x<temperature_distribution.Count; x++) {		
				var tuple = new Tuple<Uncertain<double>, Uncertain<double>> (temperature_distribution.ElementAt(x), humidity_distribution.ElementAt(x));
				pairs.Add (tuple);
			}		
		}

		public UList<double> UncertainProgram() {
			Parser (temperature, humidity);
			AddChannelNoise ();
			return temperature_distribution;
		}

//		public Uncertain<Tuple<double,double>> UseNoisyData(Uncertain<double> t, Uncertain<double> h) {
//			var ret = from tt in t
//					  from hh in h
//					select Tuple.Create(tt, hh);
//			return ret;
//		}

//		public string sendCommand(Uncertain<Tuple<double, double>> th) {
//
//		}
//		public void ACControl (string command){
//			if (command.Equals ("on")) {
//				Console.WriteLine ("on ");
//			} else
//				Console.WriteLine ("off");
//
//		}
	}
}

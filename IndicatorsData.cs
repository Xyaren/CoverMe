/* 
 * CoverMe! - Copyright (C) 2015 Juliet_Six
 * 
 * This file is part of "CoverMe!". "CoverMe!" is licensed under
 * the Microsoft Public License. You should have received a copy
 * of this license together with this file. Otherwise, see 
 * http://www.microsoft.com/en-us/openness/licenses.aspx
 *
 */
 
using System;
using Newtonsoft.Json;

namespace CoverMe
{
	/// <summary>
	/// A class containing the data that should be retrieved from the Indicators file
	/// </summary>
	public class IndicatorsData
	{	
		// All class members deserialized from JSON-object
		
		// Must be true for other members to have good values
		public bool valid;
		
		public double compass;
		
		public double altitude_10k;
		public double altitude_hour;
		
		private IndicatorsData()
		{
			// Initialize fields to default values
			valid = false;
			compass = -1.0;
			altitude_10k = -1.0;
			altitude_hour = -1.0;
		}
		
		/// <summary>
		/// Deserializes an instance of this class from a JsonObject
		/// </summary>
		/// <param name="jsonObject">A string containing the JsonObject</param>
		/// <returns></returns>
		public static IndicatorsData FromJsonString(String jsonObject) {
			// Deserialize class from JSON
			return JsonConvert.DeserializeObject<IndicatorsData>(jsonObject);
		}
		
		public bool isValid() {
			return this.valid;
		}
		
		/// <summary>
		/// Checks wether the stored altitude is in a reasonable range
		/// </summary>
		/// <returns>True if altitude seems ok, otherwise false</returns>
		public bool isGoodAltiude() {
			return getAltitudeMeters() >= 0;
		}
		
		public int getAltitudeMeters() {
			// altitude_10k contains altitude for gauges in ft
			// If it has default value it was not present and
			// altitude was metric which is stored in altitude_hour
			if (this.altitude_10k == -1.0)
				return (int)this.altitude_hour;
			// If altitude_10k was present convert to meters and return
			else
				return (int)(this.altitude_10k * Helpers.METER_PER_FEET);
		}
		
		public int getHeading() {
			return (int)this.compass;
		}
		
		
		/// <summary>
		/// Array of directions clockwise beginning with north (N to NNW)
		/// </summary>
	    static String[] directions = {"N","NNE","NE","ENE","E","ESE","SE","SSE","S","SSW","SW","WSW","W","WNW","NW","NNW"};    
	    
	    /// <summary>
		/// Returns a direction based on the heading
		/// </summary>
		/// <param name="detail">the level of detail 0=null 1=N 2=NW 3=NNW</param>
		/// <returns>1-3 character string</returns>
   		public string getAdvancedHeading(int detail){
			//filter invalid detail level
	    	if(detail==0 || detail > 3)
	    		return null;
	    	
	    	int factor = (int) Math.Pow(2,3-(detail));
	    	// calculate array Index for HDG
	    	int index = (int) Math.Round((this.compass) / (360/(directions.Length/factor)));
	      
	    	//Prevent Out of bounds
	        if(index > directions.Length/factor)
	            index-=directions.Length/factor;
	     
        	return directions[index*factor];
   		}
	}
}

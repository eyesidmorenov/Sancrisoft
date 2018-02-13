using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sancrisoft.Models
{
	public class SancrisoftMails
	{
		[Key]
		public int IdMail { get; set; }
		[Required]
		public string To { get; set; }
		[Required]
		public string Subject { get; set; }
		public string Body { get; set; }
	}
}

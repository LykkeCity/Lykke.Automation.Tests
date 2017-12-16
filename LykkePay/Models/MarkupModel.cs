using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace LykkePay.Models
{
    public class MarkupModel
    {
        public Markup markup { get; set; }

        public MarkupModel(double percent, int pips)
        {
            markup = new Markup(percent, pips);
        }
    }

    public class Markup
    {
        public double percent { get; set; }
        public int pips { get; set; }

        public Markup(double percent, int pips)
        {
            this.percent = percent;
            this.pips = pips;
        }
    }
}

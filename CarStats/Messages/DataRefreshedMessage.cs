using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HSDHelper.Models;

namespace HSDHelper.Messages {
    public class DataRefreshedMessage {
        public DataRefreshedMessage(CarState state) {
            State = state;
        }
        public CarState State { get; private set; }
    }
}

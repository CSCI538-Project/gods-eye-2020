using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[System.Serializable]
public class Transaction {
    //must match name in json
    public List<TransactionData> transactions;
}

[System.Serializable]
public class TransactionData {
    //attributes must match names in json
    public string amount;
    public string recipient;
    public string date;
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XYS.Model
{

    public enum CardType
    {
        IDCard=1, //身份证
        BankCard, //银行卡
        QRCard,  //二维码卡片
        InsCard,  //医保卡
        Other    //其他
    }
    public class SourceCard
    {
        private CardType _cardType;
        private string _cardNo;
        private string _checkNo;
        public CardType MyType
        {
            set { this._cardType = value; }
            get { return this._cardType; }
        }
        public string CardNo
        {
            set { this._cardNo = value; }
            get { return this._cardNo; }
        }
        public string CheckNo
        {
            set { this._checkNo = value; }
            get { return this._checkNo; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matrix
{
    public class Matrix {
        public int Row = 0;
        public int Column = 0;
        public bool isSquare = false;
        public bool isCorrect = true;
        public double[][] Elem;
        public Matrix(int Row,int Column,double[][] Elem) { //Elem[行][列]
            if (!(Elem.Length == Row && Elem[0].Length == Column)) //Row,ColumnとElemが一致しなかったら
                //throw new Exception("指定された数と入力された要素数が一致していません。");
                isCorrect = false; //とりあえず作成可能にしておく。
            int n = Elem[0].Length; //とりあえず一番最初の長さを取って比較
            foreach (double[] e in Elem)
                if (e.Length != n)
                    //throw new Exception("列の数が一致していません。");
                    isCorrect = false;
            this.Row = Elem.Length;
            this.Column = Elem[0].Length;
            this.isSquare = (Row == Column);
            this.Elem = Elem;
        }
        public Matrix(int Row,int Column) {
            this.Row = Row;
            this.Column = Column;
            Elem = new double[Row][];
            for (int Count = 0; Count < Row; Count++) {
                Elem[Count] = new double[Column];
            }
        }

        public double[] GetRowAt(int Row) { //行、つまり横をゲット
            return Elem[Row - 1];
        }
        public double[] GetColumnAt(int Column) { //列、つまり縦をゲット　縦が配列になって帰って来るのに注意
            List<double> ResultList = new List<double>();
            foreach (double[] e in Elem) //すべての行の中から目的の列の要素を抽出してリストに入れる
                ResultList.Add(e[Column - 1]);
            return ResultList.ToArray(); //返り値は配列
        }
        public Matrix GetRowPiercedAt(int SkipRow) { //このインスタンスから任意の行を抜いたもの。SkipRowは1から
            SkipRow--; 
            if (SkipRow + 1 <= 0 || SkipRow + 1 > this.Row) throw new Exception("1から" + this.Row.ToString() + "までの値を入力してください");
            Matrix Result = new Matrix(this.Row - 1, this.Column); //返り値
            int n = 0; //数合わせ。飛ばすとこを過ぎたら一つ先のものを入れなければならないので
            for (int Count = 0; Count < Row - 1/*相手の数に合わせる*/; Count++) {
                if (SkipRow <= Count) n = 1; //SkipRowを超えたらこっちからあげる値を一つ飛ばす。
                Result.Elem[Count] = this.Elem[Count + n];
            }
            return Result;
        }
        public Matrix GetColumnPiercedAt(int SkipColumn) {
            SkipColumn--;
            if(SkipColumn + 1 <= 0 || SkipColumn + 1 > this.Column) throw new Exception("1から" + this.Column.ToString() + "までの値を入力してください");
            Matrix Result = new Matrix(this.Row, this.Column - 1);
            for (int RowCount = 0; RowCount < Result.Row; RowCount++) { //行一つ一つにループを回す。
                int n = 0; //数合わせ。考え方はRowと同じ
                for (int ColumnCount = 0; ColumnCount < Result.Column; ColumnCount++) {
                    if (SkipColumn <= ColumnCount) n = 1;
                    Result.Elem[RowCount][ColumnCount] = this.Elem[RowCount][ColumnCount + n];
                }
            }
            return Result;
        }
        /*public int GetRowZeroNumberAt(int CheckRow) {//1から
           int Count = 0;
            foreach (double e in GetRowAt(CheckRow))
                if (e == 0) Count++;
            return Count;
        }*/
        public double Det() {
            if (this.Row == 1 && this.Column == 1) //1*1の正方行列だったらそれが行列式になる。
                return Elem[0][0];

            double Result = 0;
            int CheckRow = 1;
            //ゼロ番目の行について調べる。今後は0が多いところにする可能性がある。
            //計算するときは-1する。
            for (int CheckColumn = 1; CheckColumn <= this.Column; CheckColumn++) {
                Result += this.Elem[CheckRow - 1][CheckColumn - 1] * (((CheckRow + CheckColumn) % 2) == 0 ? 1 : -1) * this.GetRowPiercedAt(CheckRow).GetColumnPiercedAt(CheckColumn).Det();                
            }

            return Result;
        }
       
        public void PrintElem() { //行列表示
            foreach (double[] Array in Elem) {
                foreach (double e in Array)
                    Console.Write(string.Format("{0,-5}",e));
                Console.WriteLine();
            }
        }
    }
    class Programe
    {
        static void Main(string[] args) {
            /*double[][] MatrixArray = {
                new double[] {3,1,1,2 },
                new double[] {5,1,3,4},
                new double[] {2,0,1,0 },
                new double[] {1,3,2,1},
            };*/
            Random random = new Random(100);
            double[][] MatrixArray = new double[10][];
            for (int i = 0; i < 10; i ++) {
                MatrixArray[i] = new double[10];
                for (int j = 0; j < 10; j++)
                    MatrixArray[i][j] = random.Next(-6,7);
            }
            Matrix matrix = new Matrix(10,10,MatrixArray);
            Console.WriteLine("matrix : ");
            matrix.PrintElem();
            double Det = matrix.Det();

            Console.WriteLine(Det);

            Console.WriteLine();

            Console.Write("何かキーを押してください。");
            Console.ReadKey();
        }
    }
}

using System;

namespace MRL.SSL.Common.Math
{
    public class SquareMatrix<T> : Matrix<T>
    {
        public int Dimension { get { return _Rows; } }
        private int[] pi;
        private T detOfP;
        private SquareMatrix<T> _L;
        private SquareMatrix<T> _U;
        /// <summary>
        /// Matrix L of LU decomposition.
        /// Note: At first it's NULL. use MakeLU().
        /// </summary>
        public SquareMatrix<T> L { get { return _L; } }
        /// <summary>
        /// Matrix U of LU decomposition.
        /// Note: At first it's NULL. use MakeLU().
        /// </summary>
        public SquareMatrix<T> U { get { return _U; } }

        /// <param name="OperatorType">Structure wich derived from IOperator wich do operator like add,subtract,multiply,... </param>
        public SquareMatrix(IOperator<T> OperatorType) : base(OperatorType) { detOfP = Operator.One; }

        ///<param name="dimention"> Row and column of square matrix.</param>
        /// <param name="OperatorType">Structure wich derived from IOperator wich do operator like add,subtract,multiply,... </param>
        public SquareMatrix(int dimention, IOperator<T> OperatorType) : base(dimention, dimention, OperatorType)
        {
            detOfP = Operator.One;
        }


        ///<summary>
        /// Returns determinant of this matrix.
        ///</summary>
        public T Det()
        {
            if (_L == null || ValueChanged) MakeLU();
            T det = detOfP;
            for (int i = 0; i < _Rows; i++) det = Operator.Multiply(det, _U._mat[i * _U._Cols + i]);
            return det;
        }
        ///<summary>
        /// Paste matrix v to right side of this matrix and convert it to square matrix.
        ///</summary>
        public override SquareMatrix<T> Append(Matrix<T> v)
        {
            var dim = System.Math.Max(System.Math.Max(_Rows, v.Rows), _Cols + v.Cols);
            SquareMatrix<T> t = new SquareMatrix<T>(dim, Operator);

            for (int i = 0; i < dim; i++)
            {
                for (int j = 0; j < dim; j++)
                {
                    if (i < _Rows)
                    {
                        if (j < _Cols) t._mat[i * dim + j] = _mat[i * _Cols + j];
                        else if (i < v.Rows && j < v.Cols + _Cols) t._mat[i * dim + j] = v.Data[i * v.Cols + j - _Cols];
                        else t._mat[i * dim + j] = Operator.Zero;
                    }
                    else
                    {
                        if (i < v.Rows && j >= _Cols && j < v.Cols + _Cols) t._mat[i * dim + j] = v.Data[i * v.Cols + j - _Cols];
                        else t._mat[i * dim + j] = Operator.Zero;
                    }
                }
            }
            return t;
        }
        ///<summary>
        /// Paste matrix v to the bottom side of this matrix.
        ///</summary>
        public override SquareMatrix<T> Stack(Matrix<T> v)
        {
            var dim = System.Math.Max(_Rows + v.Rows, System.Math.Max(_Cols, v.Cols));
            SquareMatrix<T> t = new SquareMatrix<T>(dim, Operator);
            for (int i = 0; i < dim; i++)
            {
                for (int j = 0; j < dim; j++)
                {
                    if (j < _Cols)
                    {
                        if (i < _Rows) t._mat[i * dim + j] = _mat[i * _Cols + j];
                        else if (j < v.Cols && i < v.Rows + _Rows) t._mat[i * dim + j] = v.Data[(i - _Rows) * v.Cols + j];
                        else t._mat[i * dim + j] = Operator.Zero;
                    }
                    else
                    {
                        if (j < v.Cols && i >= _Rows && i < v.Rows + _Rows) t._mat[i * dim + j] = v.Data[(i - _Rows) * v.Cols + j];
                        else t._mat[i * dim + j] = Operator.Zero;
                    }
                }
            }
            return t;
        }

        ///<summary>
        /// Paste matrix v to right bottom side of this matrix.
        ///</summary>
        public override SquareMatrix<T> Diagonal(Matrix<T> v)
        {
            var dim = System.Math.Max(_Rows + v.Rows, _Cols + v.Cols);
            SquareMatrix<T> t = new SquareMatrix<T>(dim, Operator);
            for (int i = 0; i < dim; i++)
            {
                for (int j = 0; j < dim; j++)
                {
                    if (i < _Rows && j < _Cols) t._mat[i * dim + j] = _mat[i * _Cols + j];
                    else if (i > _Rows && i < v.Rows + _Rows && j >= _Cols && j < v.Cols + _Cols) t._mat[i * dim + j] = v.Data[(i - _Rows) * v.Cols + j - _Cols];
                    else t._mat[i * dim + j] = Operator.Zero;
                }
            }
            return t;
        }

        /// <summary>
        /// Function returns permutation matrix "P" due to permutation vector "pi".
        /// </summary>
        public Matrix<T> GetP()
        {
            if (_L == null || ValueChanged) MakeLU();

            SquareMatrix<T> matrix = new SquareMatrix<T>(Dimension, Operator);
            for (int i = 0; i < _Rows; i++) matrix._mat[pi[i] * Dimension + i] = Operator.One;
            return matrix;
        }

        ///<returns> Invert of this matrix.</returns>
        public SquareMatrix<T> Invert()
        {
            if (_L == null || ValueChanged) MakeLU();

            SquareMatrix<T> inv = new SquareMatrix<T>(Operator);

            Matrix<T> Ei = new Matrix<T>(_Rows, 1, Operator);
            for (int i = 0; i < _Rows; i++)
            {
                Ei.Data[i * Ei.Cols] = Operator.One;
                Matrix<T> col = SolveWith(Ei);
                inv.SetCol(col, i);
                Ei.Data[i * Ei.Cols] = Operator.Zero;
            }
            return inv;
        }

        ///<summary>
        /// Function for LU decomposition.
        ///</summary>
        public void MakeLU()
        {
            if (!ValueChanged && _L != null && _U != null) return;
            MatrixBuilder<T> builder = new MatrixBuilder<T>(Operator);
            _L = builder.Identity(Dimension);
            _U = new SquareMatrix<T>(Dimension, Operator);
            for (int i = 0; i < _Rows * _Cols; i++) _U._mat[i] = _mat[i];   //Cloning this matrix to U

            pi = new int[_Rows];
            for (int i = 0; i < _Rows; i++) pi[i] = i;

            T p = Operator.Zero;
            T pom2;
            int k0 = 0;
            int pom1 = 0;

            for (int k = 0; k < _Cols - 1; k++)
            {
                p = Operator.Zero;
                for (int i = k; i < _Rows; i++)      // find the row with the biggest pivot
                {
                    if (Operator.Greater(Operator.Abs(_U._mat[i * _U._Cols + k]), p))
                    {
                        p = Operator.Abs(_U._mat[i * _U._Cols + k]);
                        k0 = i;
                    }
                }
                if (Operator.Equal(p, Operator.Zero)) // samé nuly ve sloupci
                    throw new Exception("The matrix is singular!");

                pom1 = pi[k]; pi[k] = pi[k0]; pi[k0] = pom1;    // switch two rows in permutation matrix

                for (int i = 0; i < k; i++)
                {
                    pom2 = _L._mat[k * _L._Cols + i]; _L._mat[k * _L._Cols + i] = _L._mat[k0 * _L._Cols + i]; _L._mat[k0 * _L._Cols + i] = pom2;
                }

                if (k != k0) detOfP = Operator.Multiply(detOfP, Operator.NegativeOne);

                for (int i = 0; i < _Cols; i++)                  // Switch rows in U
                {
                    pom2 = _U._mat[k * _U._Cols + i]; _U._mat[k * _U._Cols + i] = _U._mat[k0 * _U._Cols + i]; _U._mat[k0 * _U._Cols + i] = pom2;
                }

                for (int i = k + 1; i < _Rows; i++)
                {
                    _L._mat[i * _L._Cols + k] = Operator.Dvide(_U._mat[i * _U._Cols + k], _U._mat[k * _U._Cols + k]);
                    for (int j = k; j < _Cols; j++)
                        _U._mat[i * _U._Cols + j] = Operator.Sub(_U._mat[i * _U._Cols + j], Operator.Multiply(_L._mat[i * _L._Cols + k], _U._mat[k * _U._Cols + j]));
                }
            }
            ValueChanged = false;
        }

        ///<summary>
        ///Remove ith row and jth col.
        ///</summary>
        public new void Reduce(int irows, int jcols)
        {
            if (jcols >= _Cols || jcols < 0) throw new Exception("Wrong col index");
            if (irows >= _Rows || irows < 0) throw new Exception("Wrong row index");
            //remove icols
            Matrix<T> t = new Matrix<T>(_Rows, _Cols - 1, Operator);
            for (int i = 0; i < _Rows; i++)
            {
                for (int j = 0, j1 = 0; j < _Cols; j++, j1++)
                {
                    if (j == jcols) { j1--; continue; }
                    t.Data[i * t.Cols + j1] = _mat[i * _Cols + j];
                }
            }
            _Cols = t.Cols; _Rows = t.Rows; _mat = t.Data;
            //remove irows
            Matrix<T> t2 = new Matrix<T>(_Rows - 1, _Cols, Operator);
            int i1 = 0;
            for (int i = 0; i < _Rows; i++, i1++)
            {
                if (i == irows) { i1--; continue; }
                for (int j = 0; j < _Cols; j++)
                    t2.Data[i1 * t2.Cols + j] = _mat[i * _Cols + j];
            }
            _Cols = t2.Cols; _Rows = t2.Rows; _mat = t2.Data; ValueChanged = true;
        }

        ///<summary>
        /// Note: Don't use this for square matrix.
        /// For square matrix use Reduce(int irows, int jcols).
        ///</summary>
        public override void RemoveCol(int col) => throw new Exception("For SquareMatrix use Reduce function!");

        ///<summary>
        /// Note: Don't use this for square matrix.
        /// For square matrix use Reduce(int irows, int jcols).
        ///</summary>
        public override void RemoveRow(int row) => throw new Exception("For SquareMatrix use Reduce function!");

        ///<summary>
        /// Function solves Ax = v in confirmity with solution vector "v".
        ///</summary>
        public Matrix<T> SolveWith(Matrix<T> v)
        {
            if (_Rows != v.Rows) throw new Exception("Wrong number of results in solution vector!");
            if (_L == null || ValueChanged) MakeLU();

            Matrix<T> b = new Matrix<T>(_Rows, 1, Operator);
            for (int i = 0; i < _Rows; i++) b.Data[i * b.Cols] = v.Data[pi[i] * v.Cols];   // switch two items in "v" due to permutation matrix

            Matrix<T> z = SubsForth(_L, b);
            Matrix<T> x = SubsBack(_U, z);

            return x;
        }


        ///<summary>
        /// Function solves Ax = b for A as an upper triangular matrix.
        /// A is this matrix.
        ///</summary>
        public Matrix<T> SubsBack(Matrix<T> b)
        {
            Matrix<float> mm = new Matrix<float>(new FloatOperator());

            if (_L == null || ValueChanged) MakeLU();
            int n = _Rows;
            Matrix<T> x = new Matrix<T>(n, 1, Operator);

            for (int i = n - 1; i > -1; i--)
            {
                x.Data[i * x.Cols] = b.Data[i * b.Cols];
                for (int j = n - 1; j > i; j--) x.Data[i * x.Cols] = Operator.Sub(x.Data[i * x.Cols], Operator.Multiply(_mat[i * _Cols + j], x.Data[j * x.Cols]));
                x.Data[i * x.Cols] = Operator.Dvide(x.Data[i * x.Cols], _mat[i * _Cols + i]);
            }
            return x;
        }

        ///<summary>
        /// Function solves Ax = b for A as a lower triangular matrix.
        /// A is this matrix.
        ///</summary>
        public Matrix<T> SubsForth(Matrix<T> b)
        {
            if (_L == null || ValueChanged) MakeLU();
            int n = _Rows;
            Matrix<T> x = new Matrix<T>(n, 1, Operator);

            for (int i = 0; i < n; i++)
            {
                x.Data[i * x.Cols] = b.Data[i * b.Cols];
                for (int j = 0; j < i; j++) x.Data[i * x.Cols] = Operator.Sub(x.Data[i * x.Cols], Operator.Multiply(_mat[i * _Cols + j], x.Data[j * x.Cols]));
                x.Data[i * x.Cols] = Operator.Dvide(x.Data[i * x.Cols], _mat[i * _Cols + i]);
            }
            return x;
        }

    }
}
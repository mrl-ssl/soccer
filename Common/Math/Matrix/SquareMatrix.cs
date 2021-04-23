using System;
using MRL.SSL.Common.Math.Helpers;

namespace MRL.SSL.Common.Math
{
    public class SquareMatrix<T> : Matrix<T>
    {
        private int[] pi;
        private T detOfP;
        private SquareMatrix<T> _l;
        private SquareMatrix<T> _u;

        public int Dimention { get; }
        /// <summary>
        /// Matrix L of LU decomposition.
        /// Note: At first it's NULL. use MakeLU().
        /// </summary>
        public SquareMatrix<T> L { get { return _l; } }
        /// <summary>
        /// Matrix U of LU decomposition.
        /// Note: At first it's NULL. use MakeLU().
        /// </summary>
        public SquareMatrix<T> U { get { return _u; } }

        ///<param name="dimention"> Row and column of square matrix.</param>
        public SquareMatrix(int dimention) : base(dimention, dimention)
        {
            Dimention = dimention; detOfP = type_helper.One;
        }

        ///<param name="dimention"> Row and column of square matrix.</param>
        /// <param name="type_helper">Structure wich derived from IGenericMathHelper wich do operations like add,subtract,multiply,... </param>
        public SquareMatrix(int dimention, IGenericMathHelper<T> type_helper) : base(dimention, dimention, type_helper)
        {
            Dimention = dimention; detOfP = type_helper.One;
        }

        /// <summary>
        /// Create matrix from array source.
        /// note : this constructor does not create new array
        /// </summary>
        /// <param name="dimention">Row and column of square matrix</param>
        /// <param name="mat">source array</param>
        /// <param name="type_helper">Structure wich derived from IGenericMathHelper wich do operations like add,subtract,multiply,...</param>
        public SquareMatrix(int dimention, T[] mat, IGenericMathHelper<T> type_helper) : base(dimention, dimention, mat, type_helper)
        {
            Dimention = dimention; detOfP = type_helper.One;
        }

        ///<summary>
        /// Returns determinant of this matrix.
        ///</summary>
        public T Det()
        {
            if (_l == null || ValueChanged) MakeLU();
            T det = detOfP;
            for (int i = 0; i < _rows; i++) det = type_helper.Multi(det, _u._mat[i * _u._cols + i]);
            return det;
        }

        /// <summary>
        /// Function returns permutation matrix "P" due to permutation vector "pi".
        /// </summary>
        public Matrix<T> GetP()
        {
            if (_l == null || ValueChanged) MakeLU();

            SquareMatrix<T> matrix = new SquareMatrix<T>(Dimention, type_helper);
            for (int i = 0; i < _rows; i++) matrix._mat[pi[i] * Dimention + i] = type_helper.One;
            return matrix;
        }

        ///<returns> Invert of this matrix.</returns>
        public SquareMatrix<T> Invert()
        {
            if (_l == null || ValueChanged) MakeLU();

            SquareMatrix<T> inv = new SquareMatrix<T>(Dimention, type_helper);

            for (int i = 0; i < _rows; i++)
            {
                Matrix<T> Ei = new Matrix<T>(_rows, 1, type_helper);
                Ei.Data[i * Ei.Cols] = type_helper.One;
                Matrix<T> col = SolveWith(Ei);
                inv.SetCol(col, i);
            }
            return inv;
        }

        ///<summary>
        /// Function for LU decomposition.
        ///</summary>
        public void MakeLU()
        {
            if (!ValueChanged && _l != null && _u != null) return;
            _l = matrixBuilder.Identity(Dimention);
            _u = new SquareMatrix<T>(Dimention, type_helper);
            for (int i = 0; i < _rows * _cols; i++) _u._mat[i] = _mat[i];   //Cloning this matrix to U

            pi = new int[_rows];
            for (int i = 0; i < _rows; i++) pi[i] = i;

            T p = type_helper.Zero;
            T pom2;
            int k0 = 0;
            int pom1 = 0;

            for (int k = 0; k < _cols - 1; k++)
            {
                p = type_helper.Zero;
                for (int i = k; i < _rows; i++)      // find the row with the biggest pivot
                {
                    if (type_helper.Greater(type_helper.Abs(_u._mat[i * _u._cols + k]), p))
                    {
                        p = type_helper.Abs(_u._mat[i * _u._cols + k]);
                        k0 = i;
                    }
                }
                if (type_helper.Equal(p, type_helper.Zero)) // samé nuly ve sloupci
                    throw new Exception("The matrix is singular!");

                pom1 = pi[k]; pi[k] = pi[k0]; pi[k0] = pom1;    // switch two rows in permutation matrix

                for (int i = 0; i < k; i++)
                {
                    pom2 = _l._mat[k * _l._cols + i]; _l._mat[k * _l._cols + i] = _l._mat[k0 * _l._cols + i]; _l._mat[k0 * _l._cols + i] = pom2;
                }

                if (k != k0) detOfP = type_helper.Multi(detOfP, type_helper.NegativeOne);

                for (int i = 0; i < _cols; i++)                  // Switch rows in U
                {
                    pom2 = _u._mat[k * _u._cols + i]; _u._mat[k * _u._cols + i] = _u._mat[k0 * _u._cols + i]; _u._mat[k0 * _u._cols + i] = pom2;
                }

                for (int i = k + 1; i < _rows; i++)
                {
                    _l._mat[i * _l._cols + k] = type_helper.Dvide(_u._mat[i * _u._cols + k], _u._mat[k * _u._cols + k]);
                    for (int j = k; j < _cols; j++)
                        _u._mat[i * _u._cols + j] = type_helper.Sub(_u._mat[i * _u._cols + j], type_helper.Multi(_l._mat[i * _l._cols + k], _u._mat[k * _u._cols + j]));
                }
            }
            ValueChanged = false;
        }

        ///<summary>
        /// Function solves Ax = v in confirmity with solution vector "v".
        ///</summary>
        public Matrix<T> SolveWith(Matrix<T> v)
        {
            if (_rows != v.Rows) throw new Exception("Wrong number of results in solution vector!");
            if (_l == null || ValueChanged) MakeLU();

            Matrix<T> b = new Matrix<T>(_rows, 1, type_helper);
            for (int i = 0; i < _rows; i++) b.Data[i * b.Cols] = v.Data[pi[i] * v.Cols];   // switch two items in "v" due to permutation matrix

            Matrix<T> z = SubsForth(_l, b);
            Matrix<T> x = SubsBack(_u, z);

            return x;
        }

        ///<summary>
        /// Function solves Ax = b for A as an upper triangular matrix.
        /// A is this matrix.
        ///</summary>
        public Matrix<T> SubsBack(Matrix<T> b)
        {
            if (_l == null || ValueChanged) MakeLU();
            int n = _rows;
            Matrix<T> x = new Matrix<T>(n, 1, type_helper);

            for (int i = n - 1; i > -1; i--)
            {
                x.Data[i * x.Cols] = b.Data[i * b.Cols];
                for (int j = n - 1; j > i; j--) x.Data[i * x.Cols] = type_helper.Sub(x.Data[i * x.Cols], type_helper.Multi(_mat[i * _cols + j], x.Data[j * x.Cols]));
                x.Data[i * x.Cols] = type_helper.Dvide(x.Data[i * x.Cols], _mat[i * _cols + i]);
            }
            return x;
        }

        ///<summary>
        /// Function solves Ax = b for A as a lower triangular matrix.
        /// A is this matrix.
        ///</summary>
        public Matrix<T> SubsForth(Matrix<T> b)
        {
            if (_l == null || ValueChanged) MakeLU();
            int n = _rows;
            Matrix<T> x = new Matrix<T>(n, 1, type_helper);

            for (int i = 0; i < n; i++)
            {
                x.Data[i * x.Cols] = b.Data[i * b.Cols];
                for (int j = 0; j < i; j++) x.Data[i * x.Cols] = type_helper.Sub(x.Data[i * x.Cols], type_helper.Multi(_mat[i * _cols + j], x.Data[j * x.Cols]));
                x.Data[i * x.Cols] = type_helper.Dvide(x.Data[i * x.Cols], _mat[i * _cols + i]);
            }
            return x;
        }
    }
}
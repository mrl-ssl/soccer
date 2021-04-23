using System;
using MRL.SSL.Common.Math.Helpers;

namespace MRL.SSL.Common.Math
{
    public class Matrix<T>
    {
        protected readonly IGenericMathHelper<T> type_helper;
        protected readonly MatrixBuilder<T> matrixBuilder;
        protected bool ValueChanged = false;
        protected int _rows, _cols;
        protected T[] _mat;

        /// <summary>
        /// Number of rows.
        /// </summary>
        public int Rows { get => _rows; }
        /// <summary>
        /// Number of columns.
        /// </summary>
        public int Cols { get => _cols; }
        /// <summary>
        /// Elements of this matrix as 1D array.
        /// </summary>
        internal T[] Data { get => _mat; }
        /// <summary>
        /// Elements of this matrix as 1D array.
        /// </summary>
        public T[] Elements
        {
            get => (T[])_mat.Clone();
        }
        public T this[int iRow, int iCol]      // Access this matrix as a 2D array
        {
            get { return _mat[iRow * Cols + iCol]; }
            set { _mat[iRow * Cols + iCol] = value; ValueChanged = true; }
        }
        public T this[int index]                // Access this matrix as a 1D array
        {
            get { return _mat[index]; }
            set { _mat[index] = value; ValueChanged = true; }
        }

        /// <param name="rows">Number of rows</param>
        /// <param name="cols">Number of columns</param>
        public Matrix(int rows, int cols)
        {
            _rows = rows; _cols = cols; _mat = new T[cols * rows];
            type_helper = MathHelper.CreateGenericMathHelper<T>();
            matrixBuilder = new MatrixBuilder<T>(type_helper);
        }

        /// <param name="rows">Number of row.</param>
        /// <param name="cols">Number of column.</param>
        /// <param name="type_helper">Structure wich do operations like add,subtract,multiply,... for given type </param>
        public Matrix(int rows, int cols, IGenericMathHelper<T> type_helper)
        {
            _rows = rows; _cols = cols; _mat = new T[cols * rows];
            if (type_helper == null)
                this.type_helper = MathHelper.CreateGenericMathHelper<T>();
            else
                this.type_helper = type_helper;
            matrixBuilder = new MatrixBuilder<T>(this.type_helper);
        }

        /// <summary>
        /// Create matrix from array source.
        /// note : this constructor does not create new array
        /// </summary>
        /// <param name="rows">Number of row</param>
        /// <param name="cols">Number of column</param>
        /// <param name="mat">source array</param>
        /// <param name="type_helper">Structure wich do operations like add,subtract,multiply,... for given type</param>
        public Matrix(int rows, int cols, T[] mat, IGenericMathHelper<T> type_helper)
        {
            _rows = rows; _cols = cols; _mat = mat;
            if (type_helper == null)
                this.type_helper = MathHelper.CreateGenericMathHelper<T>();
            else
                this.type_helper = type_helper;
            matrixBuilder = new MatrixBuilder<T>(type_helper);
        }

        protected void CopyFieldsFrom(Matrix<T> src)
        {
            _cols = src._cols; _rows = src._rows; _mat = src._mat; ValueChanged = true;
        }

        ///<summary>
        /// Paste matrix v to right side of this matrix.
        ///</summary>
        public Matrix<T> Append(Matrix<T> v)
        {
            Matrix<T> t = new Matrix<T>(System.Math.Max(_rows, v._rows), _cols + v._cols, type_helper);
            for (int i = 0; i < t._rows; i++)
            {
                for (int j = 0; j < t._cols; j++)
                {
                    if (i >= _rows)
                    {
                        if (j >= _cols) { t._mat[i * t._cols + j] = v._mat[i * v._cols + j - _cols]; }
                        else { t._mat[i * t._cols + j] = type_helper.Zero; }
                    }
                    else if (i >= v._rows)
                    {
                        if (j >= _cols) { t._mat[i * t._cols + j] = type_helper.Zero; }
                        else { t._mat[i * t._cols + j] = _mat[i * _cols + j]; }
                    }
                    else
                    {
                        if (j >= _cols) { t._mat[i * t._cols + j] = v._mat[i * v._cols + j - _cols]; }
                        else { t._mat[i * t._cols + j] = _mat[i * _cols + j]; }
                    }
                }
            }
            return t;
        }

        ///<summary>
        /// Fill all elements with zero.
        ///</summary>
        public void Clear()
        {
            for (int i = 0; i < _rows * _cols; i++)
                _mat[i] = type_helper.Zero;
            ValueChanged = true;
        }

        ///<summary>
        /// Fill col with zero.
        ///</summary>
        public void ClearCol(int col)
        {
            if (col >= _cols || col < 0) throw new Exception("Wrong col index");
            for (int i = 0; i < _rows; i++)
                _mat[i * _cols + col] = type_helper.Zero;
            ValueChanged = true;
        }

        ///<summary>
        /// Fill row with zero.
        ///</summary>
        public void ClearRow(int row)
        {
            if (row >= _rows || row < 0) throw new Exception("Wrong row index");
            for (int j = 0; j < _cols; j++)
                _mat[row * _cols + j] = type_helper.Zero;
            ValueChanged = true;
        }

        ///<summary>
        /// Fill elements by zero if func return true.
        ///</summary>
        public void CoerceZero(Func<T, bool> func)
        {
            for (int i = 0; i < _rows * _cols; i++)
                if (func(_mat[i]))
                    _mat[i] = type_helper.Zero;
            ValueChanged = true;
        }

        ///<summary>
        /// Fill elements less or equal than eps with zero.
        ///</summary>
        public void CoerceZero(T eps) => CoerceZero(x => type_helper.LessOrEqual(x, eps));

        ///<summary>
        /// Function copy matrix v elements as far as possible to this matrix.
        ///</summary>
        public void CopyFrom(Matrix<T> v)
        {
            if (_rows == v._rows && _cols == v._cols)
                for (int i = 0; i < _rows * _cols; i++)
                    _mat[i] = v._mat[i];
            else
                for (int i = 0; i < _rows; i++)
                    for (int j = 0; j < _cols; j++)
                        if (i < v._rows && j < v._cols) _mat[i * _cols + j] = v._mat[i * v._cols + j];
                        else _mat[i * _cols + j] = type_helper.Zero;
            ValueChanged = true;
        }

        ///<summary>
        /// Paste matrix v to right bottom side of this matrix.
        ///</summary>
        public Matrix<T> Diagonal(Matrix<T> v)
        {
            Matrix<T> t = new Matrix<T>(_rows + v._rows, _cols + v._cols, type_helper);
            for (int i = 0; i < t._rows; i++)
            {
                for (int j = 0; j < t._cols; j++)
                {
                    if (i < _rows && j < _cols) { t._mat[i * t._cols + j] = _mat[i * _cols + j]; }
                    else if (i >= _rows && j >= _cols) { t._mat[i * t._cols + j] = v._mat[(i - _rows) * v._cols + j - _cols]; }
                    else { t._mat[i * t._cols + j] = type_helper.Zero; }
                }
            }
            return t;
        }

        /// <returns>
        /// The copy of this matrix.
        /// </returns>
        public Matrix<T> Duplicate()
        {
            Matrix<T> matrix = new Matrix<T>(_rows, _cols, type_helper);
            for (int i = 0; i < _rows * _cols; i++)
                matrix._mat[i] = _mat[i];
            return matrix;
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is Matrix<T> matrix && matrix._cols == _cols && matrix._rows == _rows)
            {
                for (int i = 0; i < _rows * _cols; i++)
                    if (!type_helper.Equal(_mat[i], matrix._mat[i])) return false;
                return true;
            }
            return false;
        }



        /// <returns>
        /// Column number k as new (n,1) matrix.
        /// </returns>
        public Matrix<T> GetCol(int k)
        {
            Matrix<T> m = new Matrix<T>(_rows, 1, type_helper);
            for (int i = 0; i < _rows; i++) m._mat[i * m._cols] = _mat[i * _cols + k];
            return m;
        }

        public override int GetHashCode() => HashCode.Combine(_rows, _cols, _mat);

        /// <returns>
        /// Row number k as new (1,n) matrix.
        /// </returns>
        public Matrix<T> GetRow(int k)
        {
            Matrix<T> m = new Matrix<T>(1, _cols, type_helper);
            for (int i = 0; i < _cols; i++) m._mat[i] = _mat[k * _cols + i];
            return m;
        }

        /// <returns>
        /// Matrix wich filled by function func.
        /// </returns>
        public Matrix<T> Map(Func<T, T> func)
        {
            Matrix<T> t = new Matrix<T>(_rows, _cols, type_helper);
            for (int i = 0; i < _rows * _cols; i++)
                t._mat[i] = func(_mat[i]);
            return t;
        }

        ///<summary>
        /// Fill elements with function func.
        ///</summary>
        public void MapInPlace(Func<T, T> func)
        {
            for (int i = 0; i < _rows * _cols; i++) _mat[i] = func(_mat[i]);
            ValueChanged = true;
        }

        ///<summary>
        /// Remove ith row and jth col.
        ///</summary>
        public void Reduce(int irows, int jcols)
        {
            CopyFieldsFrom(RemoveCol(jcols));
            CopyFieldsFrom(RemoveRow(irows));
        }

        ///<summary>
        /// Remove col from this matrix.
        ///</summary>
        public Matrix<T> RemoveCol(int col)
        {
            if (col >= _cols || col < 0) throw new Exception("Wrong col index");
            Matrix<T> t = new Matrix<T>(_rows, _cols - 1, type_helper);
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0, j1 = 0; j < _cols; j++, j1++)
                {
                    if (j == col) { j1--; continue; }
                    t._mat[i * t._cols + j1] = _mat[i * _cols + j];
                }
            }
            return t;
        }

        ///<summary>
        /// Remove row from this matrix.
        ///</summary>
        public Matrix<T> RemoveRow(int row)
        {
            if (row >= _rows || row < 0) throw new Exception("Wrong row index");
            Matrix<T> t = new Matrix<T>(_rows - 1, _cols, type_helper);
            int i1 = 0;
            for (int i = 0; i < _rows; i++, i1++)
            {
                if (i == row) { i1--; continue; }
                for (int j = 0; j < _cols; j++)
                    t._mat[i1 * t._cols + j] = _mat[i * _cols + j];
            }
            return t;
        }

        ///<summary>
        /// Set first col of matrix v to col k of this matrix.
        ///</summary>
        public void SetCol(Matrix<T> v, int k)
        {
            if (v._rows != _rows) throw new Exception("Rows must be same.");
            for (int i = 0; i < _rows; i++) _mat[i * _cols + k] = v._mat[i * v._cols];
            ValueChanged = true;
        }

        ///<summary>
        /// Set first row of v to row k of this matrix.
        ///</summary>
        public void SetRow(Matrix<T> v, int k)
        {
            if (v._cols != _cols) throw new Exception("Cols must be same");
            for (int i = 0; i < _cols; i++) _mat[k * _cols + i] = v._mat[i];
            ValueChanged = true;
        }

        ///<summary>
        /// Paste matrix v to bottom of this matrix.
        ///</summary>
        public Matrix<T> Stack(Matrix<T> v)
        {
            Matrix<T> t = new Matrix<T>(_rows + v._rows, System.Math.Max(_cols, v._cols), type_helper);
            for (int i = 0; i < t._rows; i++)
            {
                for (int j = 0; j < t._cols; j++)
                {
                    if (j >= v._cols)
                    {
                        if (i >= _rows) { t._mat[i * t._cols + j] = type_helper.Zero; }
                        else { t._mat[i * t._cols + j] = _mat[i * _cols + j]; }
                    }
                    else if (j >= _cols)
                    {
                        if (i >= _rows) { t._mat[i * t._cols + j] = v._mat[(i - _rows) * v._cols + j]; }
                        else { t._mat[i * t._cols + j] = type_helper.Zero; }
                    }
                    else
                    {
                        if (i >= _rows) { t._mat[i * t._cols + j] = v._mat[(i - _rows) * v._cols + j]; }
                        else { t._mat[i * t._cols + j] = _mat[i * _cols + j]; }
                    }
                }
            }
            return t;
        }

        /// <returns>
        /// This matrix as square matrix.
        /// </returns>
        public SquareMatrix<T> ToSquareMatrix()
        {
            if (_rows == _cols)
                return new SquareMatrix<T>(_rows, _mat, type_helper);
            SquareMatrix<T> m = new SquareMatrix<T>(System.Math.Max(_rows, _cols), type_helper);
            m.CopyFrom(this);
            return m;
        }

        ///<summary>
        /// Function returns matrix as a string
        ///</summary>
        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < _rows; i++)
            {
                for (int j = 0; j < _cols; j++) s += String.Format("{0,5:0.00}", _mat[i * _cols + j]) + " ";
                s += "\r\n";
            }
            return s;
        }

        /// <returns>
        /// Transpose of this matrix.
        /// </returns>
        public Matrix<T> Transpose()
        {
            Matrix<T> t = new Matrix<T>(_cols, _rows, type_helper);
            for (int i = 0; i < _rows; i++)
                for (int j = 0; j < _cols; i++)
                    t._mat[j * t._cols + i] = _mat[i * _cols + j];
            return t;
        }

        ///<summary>
        /// Function solves Ax = b for A as a lower triangular matrix.
        ///</summary>
        public static Matrix<T> SubsForth(SquareMatrix<T> A, Matrix<T> b) => A.SubsForth(b);

        ///<summary>
        /// Function solves Ax = b for A as an upper triangular matrix.
        ///</summary>
        public static Matrix<T> SubsBack(SquareMatrix<T> A, Matrix<T> b) => A.SubsBack(b);

        public static Matrix<T> operator -(Matrix<T> m)
        {
            Matrix<T> r = new Matrix<T>(m._rows, m._cols, m.type_helper);
            for (int i = 0; i < r._rows * r._cols; i++)
                r._mat[i] = m.type_helper.Multi(m._mat[i], m.type_helper.NegativeOne);
            return r;
        }

        public static Matrix<T> operator +(Matrix<T> m1, Matrix<T> m2)
        {
            if (m1._rows != m2._rows || m1._cols != m2._cols) throw new Exception("Matrices must have the same dimensions!");
            Matrix<T> r = new Matrix<T>(m1._rows, m1._cols, m1.type_helper);
            for (int i = 0; i < r._rows * r._cols; i++)
                r._mat[i] = m1.type_helper.Sum(m1._mat[i], m2._mat[i]);
            return r;
        }

        public static Matrix<T> operator -(Matrix<T> m1, Matrix<T> m2)
        {
            if (m1._rows != m2._rows || m1._cols != m2._cols) throw new Exception("Matrices must have the same dimensions!");
            Matrix<T> r = new Matrix<T>(m1._rows, m1._cols, m1.type_helper);
            for (int i = 0; i < r._rows * r._cols; i++)
                r._mat[i] = m1.type_helper.Sub(m1._mat[i], m2._mat[i]);
            return r;
        }

        public static Matrix<T> operator *(Matrix<T> m1, Matrix<T> m2)
        {
            if (m1._cols != m2._rows) throw new Exception("Wrong dimensions of matrix!");
            Matrix<T> result = new Matrix<T>(m1.Rows, m2.Cols, m1.type_helper);
            for (int i = 0; i < result._rows; i++)
                for (int j = 0; j < result._cols; j++)
                    for (int k = 0; k < m1._cols; k++)
                        result._mat[i * result._cols + j] = result.type_helper.Sum(result._mat[i * result._cols + j], result.type_helper.Multi(m1._mat[i * m1._cols + k], m2._mat[k * m2._cols + j]));
            return result;
        }

        public static Matrix<T> operator *(T n, Matrix<T> m)
        {
            Matrix<T> r = new Matrix<T>(m._rows, m._cols, m.type_helper);
            for (int i = 0; i < r._rows * r._cols; i++)
                r._mat[i] = m.type_helper.Multi(m._mat[i], n);
            return r;
        }

        public static Matrix<T> operator *(Matrix<T> m, T n)
        {
            Matrix<T> r = new Matrix<T>(m._rows, m._cols, m.type_helper);
            for (int i = 0; i < r._rows * r._cols; i++)
                r._mat[i] = m.type_helper.Multi(m._mat[i], n);
            return r;
        }

        public static bool operator ==(Matrix<T> m1, Matrix<T> m2)
        {
            if (m1 is null && m2 is null) return true;
            if (m1 is null || m2 is null) return false;
            return m1.Equals(m2);
        }

        public static bool operator !=(Matrix<T> m1, Matrix<T> m2)
        {
            if (m1 is null && m2 is null) return false;
            if (m1 is null || m2 is null) return true;
            return m1.Equals(m2);
        }
    }
}
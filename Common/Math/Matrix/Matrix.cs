using System;
using System.Runtime.CompilerServices;

namespace MRL.SSL.Common.Math
{
    public class Matrix<T>
    {
        protected IOperator<T> Operator;
        protected bool ValueChanged = false;
        protected int _Rows, _Cols;
        protected T[] _mat;
        /// <summary>
        /// Number of rows.
        /// </summary>
        public int Rows { get { return _Rows; } }
        /// <summary>
        /// Number of columns.
        /// </summary>
        public int Cols { get { return _Cols; } }
        /// <summary>
        /// Elements of this matrix as 1D array.
        /// </summary>
        internal T[] Data
        {
            get { return _mat; }
        }

        /// <summary>
        /// Elements of this matrix as 1D array.
        /// </summary>
        public T[] Elements
        {
            get { return (T[])_mat.Clone(); }
        }

        /// <param name="rows">Number of row.</param>
        /// <param name="cols">Number of column.</param>
        /// <param name="OperatorType">Structure wich derived from IOperator wich do operator like add,subtract,multiply,... </param>
        public Matrix(int rows, int cols, IOperator<T> OperatorType)
        {
            _Rows = rows; _Cols = cols;
            _mat = new T[cols * rows];
            Operator = OperatorType;
        }
        public Matrix(Matrix<T> source)
        {
            if (source == null) throw new NullReferenceException("source matrix is null");
            _Rows = source._Rows; _Cols = source._Cols;
            _mat = new T[_Rows * _Cols];
            Operator = source.Operator;
            for (int i = 0; i < _Rows * _Cols; i++)
            {
                _mat[i] = source._mat[i];
            }
        }
        /// <param name="OperatorType">Structure wich derived from IOperator.</param>
        public Matrix(IOperator<T> OperatorType) { Operator = OperatorType; }
        /// <param name="OperatorType">Structure wich derived from IOperator.</param>
        public Matrix(int rows, int cols, T[] m, IOperator<T> OperatorType)
        {
            _Rows = rows; _Cols = cols;
            Operator = OperatorType;
            _mat = m;
        }

        public T this[int iRow, int iCol]      // Access this matrix as a 2D array
        {
            get { return _mat[iRow * Cols + iCol]; }
            set { _mat[iRow * Cols + iCol] = value; ValueChanged = true; }
        }
        public T this[int index]
        {
            get { return _mat[index]; }
            set { _mat[index] = value; ValueChanged = true; }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetElement(int iRow, int iCol) => _mat[iRow * Cols + iCol];

        ///<summary>
        /// Paste matrix v to right side of this matrix.
        ///</summary>
        public virtual Matrix<T> Append(Matrix<T> v)
        {
            Matrix<T> t = new Matrix<T>(System.Math.Max(_Rows, v.Rows), _Cols + v.Cols, Operator);

            for (int i = 0; i < t._Rows; i++)
            {
                for (int j = 0; j < t._Cols; j++)
                {
                    if (i < _Rows)
                    {
                        if (j < _Cols) t._mat[i * t._Cols + j] = _mat[i * _Cols + j];
                        else if (i < v.Rows) t._mat[i * t._Cols + j] = v.Data[i * v.Cols + j - _Cols];
                        else t._mat[i * t._Cols + j] = Operator.Zero;
                    }
                    else
                    {
                        if (j >= _Cols && j < v.Cols + _Cols) t._mat[i * t._Cols + j] = v.Data[i * v.Cols + j - _Cols];
                        else t._mat[i * t._Cols + j] = Operator.Zero;
                    }
                }
            }
            return t;

        }

        ///<summary>
        /// Paste matrix v to bottom of this matrix.
        ///</summary>
        public virtual Matrix<T> Stack(Matrix<T> v)
        {
            Matrix<T> t = new Matrix<T>(_Rows + v.Rows, System.Math.Max(_Cols, v.Cols), Operator);
            for (int i = 0; i < t._Rows; i++)
            {
                for (int j = 0; j < t._Cols; j++)
                {
                    if (j < _Cols)
                    {
                        if (i < _Rows) t._mat[i * t._Cols + j] = _mat[i * _Cols + j];
                        else if (j < v.Cols) t._mat[i * t._Cols + j] = v.Data[(i - _Rows) * v.Cols + j];
                        else t._mat[i * t._Cols + j] = Operator.Zero;
                    }
                    else
                    {
                        if (i >= _Rows && i < v.Rows + _Rows) t._mat[i * t._Cols + j] = v.Data[(i - _Rows) * v.Cols + j];
                        else t._mat[i * t._Cols + j] = Operator.Zero;
                    }
                }
            }
            return t;
        }
        ///<summary>
        /// Paste matrix v to right bottom side of this matrix.
        ///</summary>
        public virtual Matrix<T> Diagonal(Matrix<T> v)
        {
            Matrix<T> t = new Matrix<T>(_Rows + v.Rows, _Cols + v.Cols, Operator);
            for (int i = 0; i < t._Rows; i++)
            {
                for (int j = 0; j < t._Cols; j++)
                {
                    if (i < _Rows && j < _Cols) t._mat[i * t._Cols + j] = _mat[i * _Cols + j];
                    else if (i > _Rows && j >= _Cols) t._mat[i * t._Cols + j] = v.Data[(i - _Rows) * v.Cols + j - _Cols];
                    else t._mat[i * t._Cols + j] = Operator.Zero;
                }
            }
            return t;
        }
        ///<summary>
        /// Fill all elements with zero.
        ///</summary>
        public void Clear()
        {
            for (int i = 0; i < _Rows * _Cols; i++)
                _mat[i] = Operator.Zero;
            ValueChanged = true;
        }

        ///<summary>
        /// Fill col with zero.
        ///</summary>
        public void ClearCol(int col)
        {
            if (col >= _Cols || col < 0) throw new Exception("Wrong col index");
            for (int i = 0; i < _Rows; i++)
                _mat[i * _Cols + col] = Operator.Zero;
            ValueChanged = true;
        }

        ///<summary>
        /// Fill row with zero.
        ///</summary>
        public void ClearRow(int row)
        {
            if (row >= _Rows || row < 0) throw new Exception("Wrong row index");
            for (int j = 0; j < _Cols; j++)
                _mat[row * _Cols + j] = Operator.Zero; ;
            ValueChanged = true;
        }

        ///<summary>
        /// Fill elements less than func with zero.
        ///</summary>
        public void CoerceZero(Func<T, bool> func)
        {
            for (int i = 0; i < _Rows * _Cols; i++)
                if (func(_mat[i]))
                    _mat[i] = Operator.Zero;
            ValueChanged = true;
        }

        ///<summary>
        /// Fill elements less or equal than eps with zero.
        ///</summary>
        public void CoerceZero(T eps)
        {
            CoerceZero(x => Operator.LessOrEqual(x, eps));
        }

        ///<summary>
        /// Function copy matrix v elements as far as possible to this matrix.
        ///</summary>
        public void CopyFrom(Matrix<T> v)
        {
            for (int i = 0; i < _Rows; i++)
                for (int j = 0; j < _Cols; j++)
                    if (i < v._Rows && j < v._Cols) _mat[i * _Cols + j] = v._mat[i * v._Cols + j];
                    else _mat[i * _Cols + j] = Operator.Zero;
            ValueChanged = true;
        }


        /// <returns>
        /// copy  this matrix.
        /// </returns>
        public Matrix<T> Duplicate()
        {
            Matrix<T> matrix = new Matrix<T>(_Rows, _Cols, Operator);
            for (int i = 0; i < _Rows * _Cols; i++)
                matrix._mat[i] = _mat[i];
            return matrix;
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is Matrix<T> matrix && matrix._Cols == _Cols && matrix._Rows == _Rows)
            {
                for (int i = 0; i < _Rows * _Cols; i++)
                    if (!Operator.Equal(_mat[i], matrix._mat[i])) return false;
                return true;
            }
            return false;
        }
        public bool IsNanDiagonal()
        {
            var dim = System.MathF.Min(_Rows, _Cols);
            for (int i = 0; i < dim; i++)
            {
                if (Operator.IsNan(_mat[i * _Cols + i])) return true;
            }
            return false;
        }

        /// <returns>
        /// Col number k as matrix.
        /// </returns>
        public Matrix<T> GetCol(int k)
        {
            Matrix<T> m = new Matrix<T>(_Rows, 1, Operator);
            for (int i = 0; i < _Rows; i++) m._mat[i * m._Cols] = _mat[i * _Cols + k];
            return m;
        }

        public override int GetHashCode() => base.GetHashCode();

        /// <returns>
        /// Row number k as matrix.
        /// </returns>
        public Matrix<T> GetRow(int k)
        {
            Matrix<T> m = new Matrix<T>(1, _Cols, Operator);
            for (int i = 0; i < _Cols; i++) m._mat[i] = _mat[k * _Cols + i];
            return m;
        }

        /// <returns>
        /// Matrix wich filled by function func.
        /// </returns>
        public Matrix<T> Map(Func<T, T> func)
        {
            Matrix<T> t = new Matrix<T>(_Rows, _Cols, Operator);
            for (int i = 0; i < _Rows * _Cols; i++)
                t._mat[i] = func(_mat[i]);
            return t;
        }

        ///<summary>
        /// Fill elements with function func.
        ///</summary>
        public void MapInPlace(Func<T, T> func)
        {
            for (int i = 0; i < _Rows * _Cols; i++) _mat[i] = func(_mat[i]);
            ValueChanged = true;
        }

        ///<summary>
        /// Remove ith row and jth col.
        ///</summary>
        public virtual void Reduce(int irows, int jcols)
        {
            RemoveCol(jcols);
            RemoveRow(irows);
            ValueChanged = true;
        }

        ///<summary>
        /// Remove col from this matrix.
        ///</summary>
        public virtual void RemoveCol(int col)
        {
            if (col >= _Cols || col < 0) throw new Exception("Wrong col index");
            Matrix<T> t = new Matrix<T>(_Rows, _Cols - 1, Operator);
            for (int i = 0; i < _Rows; i++)
            {
                for (int j = 0, j1 = 0; j < _Cols; j++, j1++)
                {
                    if (j == col) { j1--; continue; }
                    t._mat[i * t._Cols + j1] = _mat[i * _Cols + j];
                }
            }
            _Cols = t._Cols; _Rows = t._Rows; _mat = t._mat;
        }

        ///<summary>
        /// Remove row from this matrix.
        ///</summary>
        public virtual void RemoveRow(int row)
        {
            if (row >= _Rows || row < 0) throw new Exception("Wrong row index");
            Matrix<T> t = new Matrix<T>(_Rows - 1, _Cols, Operator);
            int i1 = 0;
            for (int i = 0; i < _Rows; i++, i1++)
            {
                if (i == row) { i1--; continue; }
                for (int j = 0; j < _Cols; j++)
                    t._mat[i1 * t._Cols + j] = _mat[i * _Cols + j];
            }
            _Cols = t._Cols; _Rows = t._Rows; _mat = t._mat;
        }

        ///<summary>
        /// Set first col of matrix v to col k of this matrix.
        ///</summary>
        public virtual void SetCol(Matrix<T> v, int k)
        {
            if (v._Rows != _Rows) throw new Exception("Rows must be same.");
            for (int i = 0; i < _Rows; i++) _mat[i * _Cols + k] = v._mat[i * v._Cols];
            ValueChanged = true;
        }

        ///<summary>
        /// Set first row of v to row k of this matrix.
        ///</summary>
        public virtual void SetRow(Matrix<T> v, int k)
        {
            if (v._Cols != _Cols) throw new Exception("Cols must be same");
            for (int i = 0; i < _Cols; i++) _mat[k * _Cols + i] = v._mat[i];
            ValueChanged = true;
        }

        /// <returns>
        /// This matrix as square matrix.
        /// </returns>
        public SquareMatrix<T> ToSquareMatrix()
        {
            if (_Rows == _Cols) return new SquareMatrix<T>(_Rows, _mat, Operator);
            SquareMatrix<T> m = new SquareMatrix<T>(System.Math.Max(_Rows, _Cols), Operator);
            m.CopyFrom(this);
            return m;
        }

        ///<summary>
        /// Function returns matrix as a string
        ///</summary>
        public override string ToString()
        {
            string s = "";
            for (int i = 0; i < _Rows; i++)
            {
                for (int j = 0; j < _Cols; j++) s += String.Format("{0,5:0.0000000}", _mat[i * _Cols + j]) + " ";
                s += "\r\n";
            }
            return s;
        }

        /// <returns>
        /// Transpose of this matrix.
        /// </returns>
        public Matrix<T> Transpose()
        {
            Matrix<T> t = new Matrix<T>(_Cols, _Rows, Operator);
            for (int i = 0; i < t._Rows; i++)
            {
                for (int j = 0; j < t._Cols; j++)
                    t._mat[i * t._Cols + j] = _mat[j * _Cols + i];
            }
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
            Matrix<T> r = new Matrix<T>(m._Rows, m._Cols, m.Operator);
            for (int i = 0; i < r._Rows * r._Cols; i++)
                r._mat[i] = m.Operator.Multiply(m._mat[i], m.Operator.NegativeOne);
            return r;
        }

        public static Matrix<T> operator +(Matrix<T> m1, Matrix<T> m2)
        {
            if (m1._Rows != m2._Rows || m1._Cols != m2._Cols) throw new Exception("Matrices must have the same dimensions!");
            Matrix<T> r = new Matrix<T>(m1._Rows, m1._Cols, m1.Operator);
            for (int i = 0; i < r._Rows * r._Cols; i++)
                r._mat[i] = m1.Operator.Add(m1._mat[i], m2._mat[i]);
            return r;
        }

        public static Matrix<T> operator -(Matrix<T> m1, Matrix<T> m2)
        {
            if (m1._Rows != m2._Rows || m1._Cols != m2._Cols) throw new Exception("Matrices must have the same dimensions!");
            Matrix<T> r = new Matrix<T>(m1._Rows, m1._Cols, m1.Operator);
            for (int i = 0; i < r._Rows * r._Cols; i++)
                r._mat[i] = m1.Operator.Sub(m1._mat[i], m2._mat[i]);
            return r;
        }

        public static Matrix<T> operator *(Matrix<T> m1, Matrix<T> m2)
        {
            if (m1._Cols != m2._Rows) throw new Exception("Wrong dimensions of matrix!");
            Matrix<T> result = new Matrix<T>(m1.Rows, m2.Cols, m1.Operator);
            for (int i = 0; i < result._Rows; i++)
                for (int j = 0; j < result._Cols; j++)
                    for (int k = 0; k < m1._Cols; k++)
                        result._mat[i * result._Cols + j] = result.Operator.Add(result._mat[i * result._Cols + j], result.Operator.Multiply(m1._mat[i * m1._Cols + k], m2._mat[k * m2._Cols + j]));
            return result;
        }

        public static Matrix<T> operator *(T n, Matrix<T> m)
        {
            Matrix<T> r = new Matrix<T>(m._Rows, m._Cols, m.Operator);
            for (int i = 0; i < r._Rows * r._Cols; i++)
                r._mat[i] = m.Operator.Multiply(m._mat[i], n);
            return r;
        }

        public static Matrix<T> operator *(Matrix<T> m, T n)
        {
            Matrix<T> r = new Matrix<T>(m._Rows, m._Cols, m.Operator);
            for (int i = 0; i < r._Rows * r._Cols; i++)
                r._mat[i] = m.Operator.Multiply(m._mat[i], n);
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
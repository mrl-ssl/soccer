using System;

namespace MRL.SSL.Common.Math
{
    public class MatrixBuilder<T>
    {
        private IOperator<T> Operator;

        /// <param name="operatorType"> Structure wich derived from IOperator wich do operator like add,subtract,multiply,...</param>
        public MatrixBuilder(IOperator<T> operatorType) { Operator = operatorType; }

        /// <summary>
        /// Function wich generates the random matrix.
        /// </summary>
        public Matrix<T> RandomMatrix(int iRows, int iCols, T minVal, T maxVal)
        {
            Matrix<T> matrix = new Matrix<T>(iRows, iCols, Operator);
            for (int i = 0; i < iRows * iCols; i++)
                matrix.Data[i] = Operator.RandomValue(minVal, maxVal);
            return matrix;
        }
        /// <summary>
        /// Function wich generates the random square matrix.
        /// </summary>
        public SquareMatrix<T> RandomMatrix(int Dimention, T minVal, T maxVal)
        {
            SquareMatrix<T> matrix = new SquareMatrix<T>(Dimention, Operator);
            for (int i = 0; i < Dimention * Dimention; i++)
                matrix.Data[i] = Operator.RandomValue(minVal, maxVal);
            return matrix;
        }
        /// <summary>
        /// Functions wich fill elements with value.
        /// </summary>
        public Matrix<T> Dense(int iRows, int iCols, T value)
        {
            Matrix<T> matrix = new Matrix<T>(iRows, iCols, Operator);
            for (int i = 0; i < iRows * iCols; i++)
                matrix.Data[i] = value;
            return matrix;
        }
        /// <summary>
        /// Functions wich fill elements with function.
        /// </summary>
        public Matrix<T> Dence(int iRows, int iCols, Func<int, int, T> func)
        {
            Matrix<T> matrix = new Matrix<T>(iRows, iCols, Operator);
            for (int i = 0; i < iRows; i++)
                for (int j = 0; j < iCols; j++)
                    matrix.Data[i * matrix.Cols + j] = func(i, j);
            return matrix;
        }
        /// <summary>
        /// Return zero matrix.(All elements are zero)
        /// </summary>
        public Matrix<T> DenseZero(int iRows, int iCols)
        {
            Matrix<T> matrix = new Matrix<T>(iRows, iCols, Operator);
            // for (int i = 0; i < iRows * iCols; i++)
            //     matrix.Data[i] = Operator.Zero;
            return matrix;
        }
        /// <summary>
        /// Functions wich fill elements with value.
        /// </summary>
        public SquareMatrix<T> Dense(int dimention, T value)
        {
            SquareMatrix<T> matrix = new SquareMatrix<T>(dimention, Operator);
            for (int i = 0; i < dimention * dimention; i++)
                matrix.Data[i] = value;
            return matrix;
        }
        /// <summary>
        /// Functions wich fill elements with function.
        /// </summary>
        public SquareMatrix<T> Dence(int dimention, Func<int, int, T> func)
        {
            SquareMatrix<T> matrix = new SquareMatrix<T>(dimention, Operator);
            for (int i = 0; i < dimention; i++)
                for (int j = 0; j < dimention; j++)
                    matrix.Data[i * matrix.Cols + j] = func(i, j);
            return matrix;
        }
        /// <summary>
        /// Return zero matrix.(All elements are zero)
        /// </summary>
        public SquareMatrix<T> DenseZero(int dimention)
        {
            SquareMatrix<T> matrix = new SquareMatrix<T>(dimention, Operator);
            for (int i = 0; i < dimention * dimention; i++)
                matrix.Data[i] = Operator.Zero;
            return matrix;
        }
        /// <summary>
        /// Return identity matrix.(Main diameter will be filled by 1)
        /// </summary>
        public Matrix<T> Identity(int iRows, int iCols)
        {
            Matrix<T> matrix = new Matrix<T>(iRows, iCols, Operator);
            for (int i = 0; i < System.Math.Min(iRows, iCols); i++)
                matrix.Data[i * matrix.Cols + i] = Operator.One;
            return matrix;
        }
        /// <summary>
        /// Return matrix wich main diameter of that filled by value.
        /// </summary>
        /// <param name="value"> Main diameter will be filled by this.</param>
        public Matrix<T> DenceIdentity(int iRows, int iCols, T value)
        {
            Matrix<T> matrix = new Matrix<T>(iRows, iCols, Operator);
            for (int i = 0; i < System.Math.Min(iRows, iCols); i++)
                matrix.Data[i * matrix.Cols + i] = value;
            return matrix;
        }
        /// <summary>
        /// Return identity square matrix.(Main diameter will be filled by 1)
        /// </summary>
        public SquareMatrix<T> Identity(int dimention)
        {
            SquareMatrix<T> matrix = new SquareMatrix<T>(dimention, Operator);
            for (int i = 0; i < dimention; i++)
                matrix.Data[i * matrix.Cols + i] = Operator.One;
            return matrix;
        }
        /// <summary>
        /// Return matrix wich main diameter of that filled by value.
        /// </summary>
        /// <param name="value"> Main diameter will be filled by this.</param>
        public SquareMatrix<T> DenceIdentity(int dimention, T value)
        {
            SquareMatrix<T> matrix = new SquareMatrix<T>(dimention, Operator);
            for (int i = 0; i < dimention; i++)
                matrix.Data[i * matrix.Cols + i] = value;
            return matrix;
        }
    }
}
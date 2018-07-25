using System;
using System.IO;

namespace TestTask
{
    public class ReadOnlyStream : IReadOnlyStream, IDisposable
    {
        private TextReader _localStream;

        /// <summary>
        /// Конструктор класса. 
        /// Т.к. происходит прямая работа с файлом, необходимо 
        /// обеспечить ГАРАНТИРОВАННОЕ закрытие файла после окончания работы с таковым!
        /// </summary>
        /// <param name="fileFullPath">Полный путь до файла для чтения</param>
        public ReadOnlyStream(string fileFullPath)
        {
            IsEof = true;

            // TODO : Заменить на создание реального стрима для чтения файла!
            // В конструкторе инициализируем файловый и текстовый поток.
            if (String.IsNullOrEmpty(fileFullPath))
                throw new ArgumentNullException("fileFullPath");
            try
            {
                _localStream = new StreamReader(fileFullPath);
                IsEof = false;
            }
            catch
            {
                Dispose();
                _localStream = null;
            }
        }
                
        /// <summary>
        /// Флаг окончания файла.
        /// </summary>
        public bool IsEof
        {
            get; // TODO : Заполнять данный флаг при достижении конца файла/стрима при чтении
            private set;
        }

        /// <summary>
        /// Ф-ция чтения следующего символа из потока.
        /// Если произведена попытка прочитать символ после достижения конца файла, метод 
        /// должен бросать соответствующее исключение
        /// </summary>
        /// <returns>Считанный символ.</returns>
        public char ReadNextChar()
        {
            // TODO : Необходимо считать очередной символ из _localStream
            // Реализуем чтение одного символа из текстового потока.
            // После достижения коца файла закрываем потоки.
            char ch;

            try
            {
                ch = (char)_localStream.Read();
                if (_localStream.Peek() < 0)
                {
                    IsEof = true;
                    Dispose();
                }
            }
            catch
            {
                IsEof = true;
                Dispose();
                ch = Char.Parse("1");
            }
            return ch;

            //throw new NotImplementedException();
        }

        /// <summary>
        /// Сбрасывает текущую позицию потока на начало.
        /// </summary>
        public void ResetPositionToStart()
        {
            if (_localStream == null)
            {
                IsEof = true;
                return;
            }

            (_localStream as StreamReader).BaseStream.Position = 0;
            IsEof = false;
        }

        // Метод который гарантирует закрытие потоков.
        public void Dispose()
        {
            if (_localStream != null)
            {
                _localStream.Close();
                _localStream.Dispose();
                _localStream = null;
            }
        }
    }
}

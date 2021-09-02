/***************************************************************
 * File: Board.cs
 * Created By: Justin Grindal		Date: 27 June, 2013
 * Description: The main chess board. Board contain the chess cell
 * which will contain the chess pieces. Board also contains the methods
 * to get and set the user moves.
 ***************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace ChessLibrary
{
    /// <summary>
    /// he main chess board. Board contain the chess cell
    /// which will contain the chess pieces. Board also contains the methods
    /// to get and set the user moves.
    /// </summary>
    [Serializable]
    public class Board
    {
        private Side m_WhiteSide, m_BlackSide;  // Chess board site object 
        private Cells m_cells;  // collection of cells in the board
        Random rng = new Random();


        public Board()
        {
            m_WhiteSide = new Side(Side.SideType.White);	// Makde white side
            m_BlackSide = new Side(Side.SideType.Black);    // Makde white side

            m_cells = new Cells();                  // Initialize the chess cells collection
        }

        // Initialize the chess board and place piece on thier initial positions
        public void Init()
        {
            m_cells.Clear();        // Remove any existing chess cells

            // Build the 64 chess board cells
            for (int row = 1; row <= 8; row++)
                for (int col = 1; col <= 8; col++)
                {
                    m_cells.Add(new Cell(row, col));    // Initialize and add the new chess cell
                }

            // Now setup the board for black side
            m_cells["a1"].piece = new Piece(Piece.PieceType.Rook, m_BlackSide);
            m_cells["h1"].piece = new Piece(Piece.PieceType.Rook, m_BlackSide);
            m_cells["b1"].piece = new Piece(Piece.PieceType.Knight, m_BlackSide);
            m_cells["g1"].piece = new Piece(Piece.PieceType.Knight, m_BlackSide);
            m_cells["c1"].piece = new Piece(Piece.PieceType.Bishop, m_BlackSide);
            m_cells["f1"].piece = new Piece(Piece.PieceType.Bishop, m_BlackSide);
            m_cells["e1"].piece = new Piece(Piece.PieceType.King, m_BlackSide);
            m_cells["d1"].piece = new Piece(Piece.PieceType.Queen, m_BlackSide);
            for (int col = 1; col <= 8; col++)
                m_cells[2, col].piece = new Piece(Piece.PieceType.Pawn, m_BlackSide);

            // Now setup the board for white side
            m_cells["a8"].piece = new Piece(Piece.PieceType.Rook, m_WhiteSide);
            m_cells["h8"].piece = new Piece(Piece.PieceType.Rook, m_WhiteSide);
            m_cells["b8"].piece = new Piece(Piece.PieceType.Knight, m_WhiteSide);
            m_cells["g8"].piece = new Piece(Piece.PieceType.Knight, m_WhiteSide);
            m_cells["c8"].piece = new Piece(Piece.PieceType.Bishop, m_WhiteSide);
            m_cells["f8"].piece = new Piece(Piece.PieceType.Bishop, m_WhiteSide);
            m_cells["e8"].piece = new Piece(Piece.PieceType.King, m_WhiteSide);
            m_cells["d8"].piece = new Piece(Piece.PieceType.Queen, m_WhiteSide);
            for (int col = 1; col <= 8; col++)
                m_cells[7, col].piece = new Piece(Piece.PieceType.Pawn, m_WhiteSide);
        }

        public void Chess960()
        {
            m_cells.Clear();        // Remove any existing chess cells

            // Build the 64 chess board cells
            for (int row = 1; row <= 8; row++)
                for (int col = 1; col <= 8; col++)
                {
                    m_cells.Add(new Cell(row, col));    // Initialize and add the new chess cell
                }

            placeBlackSide960();
            placeWhiteSide960();
        }

        public void placeBlackSide960()
        {

            string[] blackRightRookSpaces = { "a1", "b1" };
            string[] blackLeftRookSpaces = { "g1", "h1" };
            string[] blackKingSpaces = { "c1", "d1", "e1", "f1" };

            ArrayList potentialRightBlackRookSpaces = new ArrayList(blackRightRookSpaces);
            ArrayList potentialLeftBlackRookSpaces = new ArrayList(blackLeftRookSpaces);
            ArrayList potentialBlackKingSpaces = new ArrayList(blackKingSpaces);
            ArrayList openCells = new ArrayList();

            //if rooks are placed add remaining spaces to openCells
            int getSpace = 0;
            string space = "";

            //Right Rook
            for (int i = 0; i < potentialRightBlackRookSpaces.Count; i++)
            {
                getSpace = rng.Next(potentialRightBlackRookSpaces.Count);
                space = (string)potentialRightBlackRookSpaces[getSpace];

                m_cells[space].piece = new Piece(Piece.PieceType.Rook, m_BlackSide);
                potentialRightBlackRookSpaces.Remove(space);


            }

            foreach (var openSpace in potentialRightBlackRookSpaces)
            {
                openCells.Add(openSpace);
            }

            //Left Rook
            for (int i = 0; i < potentialLeftBlackRookSpaces.Count; i++)
            {
                getSpace = rng.Next(potentialLeftBlackRookSpaces.Count);
                space = (string)potentialLeftBlackRookSpaces[getSpace];

                m_cells[space].piece = new Piece(Piece.PieceType.Rook, m_BlackSide);
                potentialLeftBlackRookSpaces.Remove(space);


            }

            foreach (var openSpace in potentialLeftBlackRookSpaces)
            {
                openCells.Add(openSpace);
            }


            //if King is placed add remaining spaces to openCells
            for (int i = 0; i < potentialBlackKingSpaces.Count; i++)
            {
                getSpace = rng.Next(potentialBlackKingSpaces.Count);
                space = (string)potentialBlackKingSpaces[getSpace];

                m_cells[space].piece = new Piece(Piece.PieceType.King, m_BlackSide);
                potentialBlackKingSpaces.Remove(space);
                break;

            }

            foreach (var openSpace in potentialBlackKingSpaces)
            {
                openCells.Add(openSpace);
            }

            //use openCells to assign to knights
            for (int i = 0; i < 2; i++)
            {
                getSpace = rng.Next(openCells.Count);
                space = (string)openCells[getSpace];

                m_cells[space].piece = new Piece(Piece.PieceType.Knight, m_BlackSide);
                openCells.Remove(space);

            }

            //use openCells to assign to bishops
            for (int i = 0; i < 2; i++)
            {
                getSpace = rng.Next(openCells.Count);
                space = (string)openCells[getSpace];

                m_cells[space].piece = new Piece(Piece.PieceType.Bishop, m_BlackSide);
                openCells.Remove(space);
            }

            //use last open cell to assign queen
            m_cells[(string)openCells[0]].piece = new Piece(Piece.PieceType.Queen, m_BlackSide);
            openCells.Remove(space);

            //place pawns
            for (int col = 1; col <= 8; col++)
                m_cells[2, col].piece = new Piece(Piece.PieceType.Pawn, m_BlackSide);

        }

        public void placeWhiteSide960()
        {
            //Random rng = new Random();

            string[] whiteRightRookSpaces = { "a8", "b8" };
            string[] whiteLeftRookSpaces = { "g8", "h8" };
            string[] whiteKingSpaces = { "c8", "d8", "e8", "f8" };

            ArrayList potentialRightWhiteRookSpaces = new ArrayList(whiteRightRookSpaces);
            ArrayList potentialLeftWhiteRookSpaces = new ArrayList(whiteLeftRookSpaces);
            ArrayList potentialWhiteKingSpaces = new ArrayList(whiteKingSpaces);
            ArrayList openCells = new ArrayList();

            //if rooks are placed add remaining spaces to openCells
            int getSpace = 0;
            string space = "";

            //Right Rook
            for (int i = 0; i < potentialRightWhiteRookSpaces.Count; i++)
            {
                getSpace = rng.Next(potentialRightWhiteRookSpaces.Count);
                space = (string)potentialRightWhiteRookSpaces[getSpace];

                m_cells[space].piece = new Piece(Piece.PieceType.Rook, m_WhiteSide);
                potentialRightWhiteRookSpaces.Remove(space);

            }

            foreach (var openSpace in potentialRightWhiteRookSpaces)
            {
                openCells.Add(openSpace);
            }

            //Left Rook
            for (int i = 0; i < potentialLeftWhiteRookSpaces.Count; i++)
            {
                getSpace = rng.Next(potentialLeftWhiteRookSpaces.Count);
                space = (string)potentialLeftWhiteRookSpaces[getSpace];

                m_cells[space].piece = new Piece(Piece.PieceType.Rook, m_WhiteSide);
                potentialLeftWhiteRookSpaces.Remove(space);

            }

            foreach (var openSpace in potentialLeftWhiteRookSpaces)
            {
                openCells.Add(openSpace);
            }


            //if white King is placed add remaining spaces to openCells
            for (int i = 0; i < potentialWhiteKingSpaces.Count; i++)
            {
                getSpace = rng.Next(potentialWhiteKingSpaces.Count);
                space = (string)potentialWhiteKingSpaces[getSpace];

                m_cells[space].piece = new Piece(Piece.PieceType.King, m_WhiteSide);
                potentialWhiteKingSpaces.Remove(space);
                break;

            }

            foreach (var openSpace in potentialWhiteKingSpaces)
            {
                openCells.Add(openSpace);
            }

            //use openCells to assign to white knights
            for (int i = 0; i < 2; i++)
            {
                getSpace = rng.Next(openCells.Count);
                space = (string)openCells[getSpace];

                m_cells[space].piece = new Piece(Piece.PieceType.Knight, m_WhiteSide);
                openCells.Remove(space);

            }

            //use openCells to assign to white bishops
            for (int i = 0; i < 2; i++)
            {
                getSpace = rng.Next(openCells.Count);
                space = (string)openCells[getSpace];

                m_cells[space].piece = new Piece(Piece.PieceType.Bishop, m_WhiteSide);
                openCells.Remove(space);
            }

            //use last open cell to assign white queen
            m_cells[(string)openCells[0]].piece = new Piece(Piece.PieceType.Queen, m_WhiteSide);
            openCells.Remove(space);

            //place white pawns
            for (int col = 1; col <= 8; col++)
                m_cells[7, col].piece = new Piece(Piece.PieceType.Pawn, m_WhiteSide);
        }

        // get the new item by rew and column
        public Cell this[int row, int col]
        {
            get
            {
                return m_cells[row, col];
            }
        }

        // get the new item by string location
        public Cell this[string strloc]
        {
            get
            {
                return m_cells[strloc];
            }
        }

        // get the chess cell by given cell
        public Cell this[Cell cellobj]
        {
            get
            {
                return m_cells[cellobj.ToString()];
            }
        }

        /// <summary>
        /// Serialize the Game object as XML String
        /// </summary>
        /// <returns>XML containing the Game object state XML</returns>
        public XmlNode XmlSerialize(XmlDocument xmlDoc)
        {
            XmlElement xmlBoard = xmlDoc.CreateElement("Board");

            // Append game state attributes
            xmlBoard.AppendChild(m_WhiteSide.XmlSerialize(xmlDoc));
            xmlBoard.AppendChild(m_BlackSide.XmlSerialize(xmlDoc));

            xmlBoard.AppendChild(m_cells.XmlSerialize(xmlDoc));

            // Return this as String
            return xmlBoard;
        }

        /// <summary>
        /// DeSerialize the Board object from XML String
        /// </summary>
        /// <returns>XML containing the Board object state XML</returns>
        public void XmlDeserialize(XmlNode xmlBoard)
        {
            // Deserialize the Sides XML
            XmlNode side = XMLHelper.GetFirstNodeByName(xmlBoard, "Side");

            // Deserialize the XML nodes
            m_WhiteSide.XmlDeserialize(side);
            m_BlackSide.XmlDeserialize(side.NextSibling);

            // Deserialize the Cells
            XmlNode xmlCells = XMLHelper.GetFirstNodeByName(xmlBoard, "Cells");
            m_cells.XmlDeserialize(xmlCells);
        }

        // get all the cell locations on the chess board
        public ArrayList GetAllCells()
        {
            ArrayList CellNames = new ArrayList();

            // Loop all the squars and store them in Array List
            for (int row = 1; row <= 8; row++)
                for (int col = 1; col <= 8; col++)
                {
                    CellNames.Add(this[row, col].ToString()); // append the cell name to list
                }

            return CellNames;
        }

        // get all the cell containg pieces of given side
        public ArrayList GetSideCell(Side.SideType PlayerSide)
        {
            ArrayList CellNames = new ArrayList();

            // Loop all the squars and store them in Array List
            for (int row = 1; row <= 8; row++)
                for (int col = 1; col <= 8; col++)
                {
                    // check and add the current type cell
                    if (this[row, col].piece != null && !this[row, col].IsEmpty() && this[row, col].piece.Side.type == PlayerSide)
                        CellNames.Add(this[row, col].ToString()); // append the cell name to list
                }

            return CellNames;
        }

        // Returns the cell on the top of the given cell
        public Cell TopCell(Cell cell)
        {
            return this[cell.row - 1, cell.col];
        }

        // Returns the cell on the left of the given cell
        public Cell LeftCell(Cell cell)
        {
            return this[cell.row, cell.col - 1];
        }

        // Returns the cell on the right of the given cell
        public Cell RightCell(Cell cell)
        {
            return this[cell.row, cell.col + 1];
        }

        // Returns the cell on the bottom of the given cell
        public Cell BottomCell(Cell cell)
        {
            return this[cell.row + 1, cell.col];
        }

        // Returns the cell on the top-left of the current cell
        public Cell TopLeftCell(Cell cell)
        {
            return this[cell.row - 1, cell.col - 1];
        }

        // Returns the cell on the top-right of the current cell
        public Cell TopRightCell(Cell cell)
        {
            return this[cell.row - 1, cell.col + 1];
        }

        // Returns the cell on the bottom-left of the current cell
        public Cell BottomLeftCell(Cell cell)
        {
            return this[cell.row + 1, cell.col - 1];
        }

        // Returns the cell on the bottom-right of the current cell
        public Cell BottomRightCell(Cell cell)
        {
            return this[cell.row + 1, cell.col + 1];
        }
    }
}

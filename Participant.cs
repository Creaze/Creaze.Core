/// This is part of the Creaze.Core project.
/// 
/// Copyright (c) 2015 Tobias Strunck <tobi1924@googlemail.com>
/// 
/// This program is free software; you can redistribute it and/or modify
/// it under the terms of the GNU General Public License as published by
/// the Free Software Foundation; either version 2 of the License, or
/// (at your option) any later version.
/// 
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
/// GNU General Public License for more details.
/// 
/// You should have received a copy of the GNU General Public License along
/// with this program; if not, write to the Free Software Foundation, Inc.,
/// 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Creaze.Core
{
    /// <summary>
    /// Member ist eine Klasse für alle Mitglieder im System.
    /// </summary>
    public class Member: IComparable<Member>, INotifyPropertyChanged, IDisposable
    {
        #region Felder
        private static int _id = 0;
        private int _myId;
        private string _firstName;
        private string _lastName;
        private bool _isManager;
        private int _count;
        private ObservableCollection<Group> _groups;
        #endregion

        #region Ereignisse
        /// <summary>
        /// Benachrichtigt Clients darüber, dass eine Eigenschaft geändert wurde.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Konstruktoren
        /// <summary>
        /// Initialisiert eine neue Instanz der Creaze.Core.Member-Klasse.
        /// </summary>
        public Member()
        {
            this._myId = _id++;
            this._groups = new ObservableCollection<Group>();
        }

        /// <summary>
        /// Initialisiert eine neue Instanz der Creaze.Core.Member-Klasse.
        /// </summary>
        /// <param name="firstName">Vorname</param>
        /// <param name="lastName">Nachname</param>
        /// <param name="isManager">Gibt an, dass dieses Mitglied ein Manager ist.</param>
        /// <param name="count">Der Job-Zähler für dieses Mitglied.</param>
        public Member(string firstName, string lastName, bool isManager = false, int count = 0)
        {
            this._myId = _id++;
            this._groups = new ObservableCollection<Group>();
            this.FirstName = firstName;
            this.LastName = lastName;
            this.IsManager = isManager;
            this.Count = count;
        }
        #endregion

        #region Eigenschaften
        /// <summary>
        /// Ruft den Vornamen ab oder legt ihn fest.
        /// </summary>
        public string FirstName
        {
            get
            {
                return this._firstName;
            }
            set
            {
                if (this._firstName != value)
                {
                    this._firstName = value;
                    OnPropertyChanged("FirstName");
                }
            }
        }

        /// <summary>
        /// Ruft den Nachnamen ab oder legt ihn fest.
        /// </summary>
        public string LastName
        {
            get
            {
                return this._lastName;
            }
            set
            {
                if (this._lastName != value)
                {
                    this._lastName = value;
                    OnPropertyChanged("LastName");
                }
            }
        }

        /// <summary>
        /// Ruft ab oder legt fest, ob es sich um einen Manager handelt.
        /// </summary>
        public bool IsManager
        {
            get
            {
                return this._isManager;
            }
            set
            {
                if (this._isManager != value)
                {
                    this._isManager = value;
                    OnPropertyChanged("IsManager");
                }
            }
        }

        /// <summary>
        /// Ruft den Job-Zähler ab oder legt ihn fest.
        /// </summary>
        public int Count
        {
            get
            {
                return this._count;
            }
            set
            {
                if (this._count != value)
                {
                    this._count = value;
                    OnPropertyChanged("Count");
                }
            }
        }

        /// <summary>
        /// Ruft die Gruppen ab, bei denen dieser Teilnehmer Mitglied ist.
        /// </summary>
        public ObservableCollection<Group> Groups
        {
            get
            {
                return this._groups;
            }
        }

        /// <summary>
        /// Ruft die Id ab.
        /// </summary>
        internal int Id
        {
            get
            {
                return this._myId;
            }
        }
        #endregion

        #region Methoden
        /// <summary>
        /// Löst das PropertyChanged-Ereignis aus.
        /// </summary>
        /// <param name="info">Die Eigenschaft, die geändert wurde.</param>
        protected virtual void OnPropertyChanged(string info)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        /// <summary>
        /// Fügt eine Gruppe hinzu.
        /// </summary>
        /// <param name="group">Die Gruppe, die hinzugefügt werden soll.</param>
        internal void SubscribeToGroup(Group group)
        {
            foreach (Group g in this._groups)
            {
                if (g.Id == group.Id)
                    return;
            }
            this._groups.Add(group);
        }

        /// <summary>
        /// Entfernt eine Gruppe.
        /// </summary>
        /// <param name="group">Die Gruppe, die entfernt werden soll.</param>
        internal void UnsubscribeFromGroup(Group group)
        {
            this._groups.Remove(group);
        }

        /// <summary>
        /// Vergleicht zwei Mitglieder miteinander.
        /// </summary>
        /// <param name="other">Das andere Mitglied, mit dem verglichen werden soll.</param>
        /// <returns>-1, wenn das andere Mitglied öfter dran war, 0, wenn sie gleich oft dran waren, oder 1, wenn das andere seltener dran war.</returns>
        public int CompareTo(Member other)
        {
            if (this.Count < other.Count)
                return -1;
            else if (this.Count > other.Count)
                return 1;
            else
                return 0;
        }

        /// <summary>
        /// Liefert eine Zeichenkette zurück, die diese Instanz repräsentiert.
        /// </summary>
        /// <returns>Die Zeichenkette für diese Instanz.</returns>
        public override string ToString()
        {
            return this.FirstName + " " + this.LastName;
        }

        /// <summary>
        /// Gibt von diesem Objekt verwendete Ressourcen frei.
        /// </summary>
        public void Dispose()
        {
            while (this._groups.Count > 0)
            {
                this._groups[this._groups.Count - 1].RemoveMember(this);
            }
        }
        #endregion
    }
}

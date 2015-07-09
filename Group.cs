/// This is part of the Creaze.Core project.
/// 
/// Copyright (c) 2015 Creaze Team and/or contributors.
/// 
/// Author: Tobias Strunck
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
    /// Group stellt eine Gruppe von Mitgliedern da.
    /// </summary>
    public class Group: IComparable<Group>, IDisposable, INotifyPropertyChanged
    {
        #region Felder
        private static int _id = 0;
        private int _myId;
        private string _name;
        private ObservableCollection<Member> _members;
        private PropertyChangedEventHandler _propertyChangedHandler;
        #endregion

        #region Ergeignisse
        /// <summary>
        /// Benachrichtigt Clients darüber, dass eine Eigenschaft geändert wurde.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Konstruktoren
        /// <summary>
        /// Initialisiert eine neue Instanz der Creaze.Core.Group-Klasse.
        /// </summary>
        public Group()
        {
            this._myId = _id++;
            this._members = new ObservableCollection<Member>();
            this._propertyChangedHandler = new PropertyChangedEventHandler(member_PropertyChanged);
        }

        /// <summary>
        /// Initialisiert eine neue Instanz der Creaze.Core.Group-Klasse.
        /// </summary>
        /// <param name="name">Der Name für die neue Gruppe.</param>
        public Group(string name)
        {
            this._myId = _id++;
            this.Name = name;
            this._members = new ObservableCollection<Member>();
            this._propertyChangedHandler = new PropertyChangedEventHandler(member_PropertyChanged);
        }
        #endregion

        #region Eigenschaften
        /// <summary>
        /// Ruft den Namen der Gruppe ab oder legt diesen fest.
        /// </summary>
        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                if (this._name != value)
                {
                    this._name = value;
                    this.OnPropertyChanged("Name");
                }
            }
        }

        /// <summary>
        /// Ruft die Anzahl an Nicht-Managern in dieser Gruppe ab.
        /// </summary>
        public int NormalCount
        {
            get
            {
                int count = 0;
                foreach (Member member in this._members)
                {
                    if (!member.IsManager)
                        count++;
                }
                return count;
            }
        }

        /// <summary>
        /// Ruft die Anzahl an Managern in dieser Gruppe ab.
        /// </summary>
        public int ManagerCount
        {
            get
            {
                int count = 0;
                foreach (Member member in this._members)
                {
                    if (member.IsManager)
                        count++;
                }
                return count;
            }
        }

        /// <summary>
        /// Ruft eine Liste mit allen Gruppen-Mitgliedern ab.
        /// </summary>
        public ObservableCollection<Member> Members
        {
            get
            {
                return this._members;
            }
        }

        /// <summary>
        /// Ruft die Id der Gruppe ab.
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
        /// Fügt ein neues Mitglied in die Gruppe ein.
        /// </summary>
        /// <param name="member">Das Mitglied, das hinzugefügt werden soll.</param>
        public void AddMember(Member member)
        {
            if (!this.IsGroupMember(member))
            {
                this._members.Add(member);
                member.SubscribeToGroup(this);
                member.PropertyChanged += this._propertyChangedHandler;
                this.OnPropertyChanged("NormalCount");
                this.OnPropertyChanged("ManagerCount");
            }
        }

        /// <summary>
        /// Event-Handler für das PropertyChanged-Event von Gruppenmitgliedern.
        /// </summary>
        /// <param name="sender">Objekt, dass das Event ausgelöst hat.</param>
        /// <param name="e">Weitere Event-Informationen.</param>
        private void member_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("IsManager"))
            {
                this.OnPropertyChanged("NormalCount");
                this.OnPropertyChanged("ManagerCount");
            }
        }

        /// <summary>
        /// Entfernt ein Mitglied aus einer Gruppe.
        /// </summary>
        /// <param name="member">Das Mitglied, das entfernt werden soll.</param>
        public void RemoveMember(Member member)
        {
            this._members.Remove(member);
            member.UnsubscribeFromGroup(this);
            member.PropertyChanged -= this._propertyChangedHandler;
            this.OnPropertyChanged("NormalCount");
            this.OnPropertyChanged("ManagerCount");
        }

        /// <summary>
        /// Prüft, ob ein Mitglied bereits Mitglied der Gruppe ist.
        /// </summary>
        /// <param name="member">Mitglied, das geprüft werden soll.</param>
        /// <returns>true, wenn es Mitglied der Gruppe ist.</returns>
        public bool IsGroupMember(Member member)
        {
            foreach (Member mem in this._members)
            {
                if (mem.Id == member.Id)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Vergleicht zwei Gruppen miteinander.
        /// </summary>
        /// <param name="other">Die andere Gruppe, mit der diese Gruppe verglichen werden soll.</param>
        /// <returns>-1, wenn die andere Gruppe größer ist, 0, wenn sie gleich sind und 1, wenn diese Gruppe die größere ist.</returns>
        public int CompareTo(Group other)
        {
            if (this.Members.Count < other.Members.Count)
                return -1;
            else if (this.Members.Count > other.Members.Count)
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
            return this.Name;
        }

        /// <summary>
        /// Gibt von diesem Objekt verwendete Ressourcen frei.
        /// </summary>
        public void Dispose()
        {
            Member member = null;
            while (this._members.Count > 0)
            {
                member = this._members[this._members.Count - 1];
                this.RemoveMember(member);
            }
        }

        /// <summary>
        /// Löst das PropertyChanged-Ergeignis aus.
        /// </summary>
        /// <param name="info">Die Eigenschaft, die geändert wurde.</param>
        protected virtual void OnPropertyChanged(string info)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
        #endregion
    }
}

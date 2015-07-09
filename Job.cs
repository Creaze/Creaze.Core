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
    /// Job stellt eine Aufagbe dar, zu der Mitglieder zugeteilt werden können.
    /// </summary>
    public class Job : IComparable<Job>, INotifyPropertyChanged, IDisposable
    {
        #region Felder
        private static int _id = 0;
        private int _myId;
        private string _name;
        private DateTime _time;
        private int _minSubscriberCount;
        private int _maxSubscriberCount;
        private int _minManagerCount;
        private int _maxManagerCount;
        private ObservableCollection<Member> _possibleSubscribers;
        private ObservableCollection<Member> _subscribers;
        private ObservableCollection<Member> _managers;
        #endregion

        #region Ereignisse
        /// <summary>
        /// Benachrichtigt Clients darüber, dass eine Eigenschaft geändert wurde.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Konstruktoren
        /// <summary>
        /// Initialisiert eine neue Instanz der Creaze.Core.Job-Klasse.
        /// </summary>
        public Job()
        {
            this._myId = _id++;
            this._subscribers = new ObservableCollection<Member>();
            this._managers = new ObservableCollection<Member>();
            this._possibleSubscribers = new ObservableCollection<Member>();
            this.Time = DateTime.Now;
        }

        /// <summary>
        /// Initialisiert eine neue Instanz der Creaze.Core.Job-Klasse.
        /// </summary>
        /// <param name="time">Der Zeitpunkt für diese Aufgabe.</param>
        /// <param name="name">Ein Name für diese Aufgabe.</param>
        public Job(DateTime time, string name = "")
        {
            this._myId = _id++;
            this._subscribers = new ObservableCollection<Member>();
            this._managers = new ObservableCollection<Member>();
            this._possibleSubscribers = new ObservableCollection<Member>();
            this.Time = time;
            this.Name = name;
        }
        #endregion

        #region Eigenschaften
        /// <summary>
        /// Ruft den Namen für diese Aufgabe ab oder legt ihn fest.
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
                    OnPropertyChanged("Name");
                }
            }
        }

        /// <summary>
        /// Legt den Zeitpunkt für diese Aufgabe fest oder ruft ihn ab.
        /// </summary>
        public DateTime Time
        {
            get
            {
                return this._time;
            }
            set
            {
                if (this._time != value)
                {
                    this._time = value;
                    OnPropertyChanged("Time");
                }
            }
        }

        /// <summary>
        /// Ruft die minimale Anzahl an Teilnehmern ab oder legt sie fest.
        /// </summary>
        public int MinSubscriberCount
        {
            get
            {
                return this._minSubscriberCount;
            }
            set
            {
                if (this._minSubscriberCount != value)
                {
                    this._minSubscriberCount = value;
                    OnPropertyChanged("MinSubscriberCount");
                }
            }
        }

        /// <summary>
        /// Ruft die maximale Anzahl an Teilnehmern ab oder legt sie fest.
        /// </summary>
        public int MaxSubscriberCount
        {
            get
            {
                return this._maxSubscriberCount;
            }
            set
            {
                if (this._maxSubscriberCount != value)
                {
                    this._maxSubscriberCount = value;
                    OnPropertyChanged("MaxSubscriberCount");
                }
            }
        }

        /// <summary>
        /// Ruft die minimale Anzahl an Managern ab oder legt sie fest.
        /// </summary>
        public int MinManagerCount
        {
            get
            {
                return this._minManagerCount;
            }
            set
            {
                if (this._minManagerCount != value)
                {
                    this._minManagerCount = value;
                    OnPropertyChanged("MinManagerCount");
                }
            }
        }

        /// <summary>
        /// Ruft die maximale Anzahl an Managern ab oder legt sie fest.
        /// </summary>
        public int MaxManagerCount
        {
            get
            {
                return this._maxManagerCount;
            }
            set
            {
                if (this._maxManagerCount != value)
                {
                    this._maxManagerCount = value;
                    OnPropertyChanged("MaxManagerCount");
                }
            }
        }

        /// <summary>
        /// Ruft die Teilnehmer für diese Aufgabe ab.
        /// </summary>
        public ObservableCollection<Member> Subscribers
        {
            get
            {
                return this._subscribers;
            }
        }

        /// <summary>
        /// Ruft die Manager für diese Aufgabe ab.
        /// </summary>
        public ObservableCollection<Member> Managers
        {
            get
            {
                return this._managers;
            }
        }

        /// <summary>
        /// Ruft die möglichen Teilnehmer für diese Aufgabe ab oder legt sie fest.
        /// </summary>
        public ObservableCollection<Member> PossibleSubscribers
        {
            get
            {
                return this._possibleSubscribers;
            }
            set
            {
                if (this._possibleSubscribers != value)
                {
                    this._possibleSubscribers = value;
                    OnPropertyChanged("PossibleSubscribers");
                }
            }
        }

        /// <summary>
        /// Ruft die Id dieses Jobs ab.
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
                this.PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        /// <summary>
        /// Fügt einen Teilnehmer, der auch Manager sein kann, hinzu.
        /// </summary>
        /// <param name="member">Der Teilnehmer, der hinzugefügt werden soll.</param>
        /// <returns>true, wenn der Teilnehmer hinzugefügt wurde.</returns>
        public bool AddSubscriber(Member member)
        {
            if (!this.IsSubscriber(member))
            {
                if (member.IsManager)
                {
                    this._managers.Add(member);
                }
                else
                {
                    this._subscribers.Add(member);
                }
                member.Count++;
                this.PossibleSubscribers.Remove(member);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Entfernt einen Teilnehmer von dieser Aufgabe.
        /// </summary>
        /// <param name="member">Der Teilnehmer, der entfernt werden soll.</param>
        public bool RemoveSubscriber(Member member)
        {
            if (member.IsManager)
            {
                if (this._managers.Remove(member))
                {
                    member.Count--;
                    this.PossibleSubscribers.Add(member);
                    return true;
                }
            }
            else
            {
                if (this._subscribers.Remove(member))
                {
                    member.Count--;
                    this.PossibleSubscribers.Add(member);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Fügt eine Gruppe von Teilnehmern zu dieser Aufgabe hinzu.
        /// </summary>
        /// <param name="group">Die Gruppe, die hinzugefügt werden soll.</param>
        /// <returns>true, wenn die Gruppe hinzugefügt wurde.</returns>
        public bool AddSubscriberGroup(Group group)
        {
            if (group.NormalCount <= (this.MaxSubscriberCount - this.Subscribers.Count) &&
                group.ManagerCount <= (this.MaxManagerCount - this.Managers.Count))
            {
                foreach (Member member in group.Members)
                {
                    if (!this.AddSubscriber(member))
                        return false;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Prüft, ob ein Mitglied bereits dieser Aufgabe zugeteilt wurde.
        /// </summary>
        /// <param name="member">Das Mitglied, das geprüft werden soll.</param>
        /// <returns>true, wenn das Mitglied bereits hinzugefügt wurde.</returns>
        public bool IsSubscriber(Member member)
        {
            if (member.IsManager)
            {
                foreach (Member item in this._managers)
                {
                    if (item.Id == member.Id)
                        return true;
                }
            }
            else
            {
                foreach (Member item in this._subscribers)
                {
                    if (item.Id == member.Id)
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Vergleicht zwei Aufgaben miteinander.
        /// </summary>
        /// <param name="other">Die Aufgabe, mit der verglichen werden soll.</param>
        /// <returns>-1, wenn diese Aufgabe vor der anderen stattfindet, 0, wenn sie gleichzeitig sind und 1, wenn sie später ist.</returns>
        public int CompareTo(Job other)
        {
            if (this.Time < other.Time)
                return -1;
            else if (this.Time > other.Time)
                return 1;
            else
                return 0;
        }

        /// <summary>
        /// Konvertiert den Wert dieser Aufgabe in eine Zeichenfolge.
        /// </summary>
        /// <returns>Die Zeichenfolge.</returns>
        public override string ToString()
        {
            return this.Time + ", " + this.Name;
        }

        /// <summary>
        /// Gibt von diesem Objekt verwendete Ressourcen frei.
        /// </summary>
        public void Dispose()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

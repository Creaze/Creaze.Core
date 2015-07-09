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
    /// Lineup stellt einen kompletten Dienstplan dar.
    /// </summary>
    public class Lineup : INotifyPropertyChanged
    {
        #region Felder
        private ObservableCollection<Member> _participants;
        private ObservableCollection<Group> _groups;
        private ObservableCollection<Job> _jobs;
        #endregion

        /// <summary>
        /// Benachrichtigt Clients darüber, dass eine Eigenschaft geändert wurde.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #region Konstruktoren
        /// <summary>
        /// Instantiiert ein neues Objekt dieser Klasse.
        /// </summary>
        public Lineup()
        {
            this._participants = new ObservableCollection<Member>();
            this._groups = new ObservableCollection<Group>();
            this._jobs = new ObservableCollection<Job>();
        }
        #endregion

        #region Eigenschaften
        /// <summary>
        /// Ruft alle Mitglieder ab oder legt sie fest.
        /// </summary>
        public ObservableCollection<Member> Members
        {
            get
            {
                return this._participants;
            }
            set
            {
                if (value != this._participants)
                {
                    this._participants = value;
                    OnPropertyChanged("Members");
                }
            }
        }

        /// <summary>
        /// Ruft alle Gruppen ab oder legt sie fest.
        /// </summary>
        public ObservableCollection<Group> Groups
        {
            get
            {
                return this._groups;
            }
            set
            {
                if (value != this._groups)
                {
                    this._groups = value;
                    OnPropertyChanged("Groups");
                }
            }
        }

        /// <summary>
        /// Ruft alle Aufgaben ab oder legt sie fest.
        /// </summary>
        public ObservableCollection<Job> Jobs
        {
            get
            {
                return this._jobs;
            }
            set
            {
                if (value != this._jobs)
                {
                    this._jobs = value;
                    OnPropertyChanged("Jobs");
                }
            }
        }
        #endregion

        #region Methoden
        /// <summary>
        /// Löst das PropertyChanged-Ereignis aus.
        /// </summary>
        /// <param name="info">Die Eigenschaft, die geändert wurde.</param>
        protected virtual void OnPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        /// <summary>
        /// Erstellt einen neuen Dienstplan.
        /// </summary>
        public void CreateLineup()
        {
            List<Job> jobs = new List<Job>();
            foreach (Job job in this.Jobs)
            {
                jobs.Add(job);
            }

            jobs.Sort(delegate(Job x, Job y)
            {
                if (x == null)
                {
                    if (y == null)
                        return 0;
                    else
                        return -1;
                }
                else
                {
                    if (y == null)
                        return 1;
                    else
                    {
                        if (x.PossibleSubscribers.Count > y.PossibleSubscribers.Count)
                            return 1;
                        else if (x.PossibleSubscribers.Count < y.PossibleSubscribers.Count)
                            return -1;
                        else
                            return 0;
                    }
                }
            });

            int offset = 3;
            int index = 0;
            Random rand = new Random();
            List<Member> members = new List<Member>();
            List<Member> managers = new List<Member>();

            foreach (Job job in jobs)
            {
                foreach (Member member in job.PossibleSubscribers)
                {
                    if (member.IsManager)
                        managers.Add(member);
                    else
                        members.Add(member);
                }

                members.Sort();
                while (job.MinSubscriberCount > job.Subscribers.Count && members.Count > 0)
                {
                    offset = members.Count > 3 ? 3 : members.Count;
                    index = rand.Next(offset);
                    if (job.IsSubscriber(members[index]))
                    {
                        members.RemoveAt(index);
                        continue;
                    }
                    else if (members[index].Groups != null && members[index].Groups.Count > 0)
                    {
                        if (!job.AddSubscriberGroup(members[index].Groups[0]))
                        {
                            job.AddSubscriber(members[index]);
                        }
                    }
                    else
                        job.AddSubscriber(members[index]);
                    members.RemoveAt(index);
                }

                managers.Sort();
                while (job.MinManagerCount > job.Managers.Count && managers.Count > 0)
                {
                    offset = managers.Count > 3 ? 3 : managers.Count;
                    index = rand.Next(offset);
                    if (job.IsSubscriber(managers[index]))
                    {
                        managers.RemoveAt(index);
                        continue;
                    }
                    else if (managers[index].Groups != null && managers[index].Groups.Count > 0)
                    {
                        if (!job.AddSubscriberGroup(managers[index].Groups[0]))
                        {
                            job.AddSubscriber(managers[index]);
                        }
                    }
                    else
                        job.AddSubscriber(managers[index]);
                    managers.RemoveAt(index);
                }

                managers.Clear();
                members.Clear();
            }
        }
        #endregion
    }
}

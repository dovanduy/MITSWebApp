import { Component, OnInit } from '@angular/core';

import { Nav } from "../models";


@Component({
  selector: 'sidenav',
  templateUrl: './sidenav.component.html',
  styleUrls: ['./sidenav.component.scss']
})
export class SidenavComponent implements OnInit {

  constructor() { }

  navs: Nav[] = [
    {
      link: '/admin/dashboard',
      name: 'Dashboard',
      icon: 'dashboard',

    },
    {
      link: '/admin/checkin',
      name: 'Check-in',
      icon: 'how_to_reg'
    },
    {
      link: '/admin/days',
      name: 'Days',
      icon: 'calendar_view_day'
    },
    {
      link: '/admin/sections',
      name: 'Sections',
      icon: 'view_headline'
    },
    {
      link: '/admin/speakers',
      name: 'Speakers',
      icon: 'person_add'
    },
    {
      link: '/admin/event',
      name: 'Wild Apricot Event',
      icon: 'event'
    },
  ]

  ngOnInit() {
  }

}

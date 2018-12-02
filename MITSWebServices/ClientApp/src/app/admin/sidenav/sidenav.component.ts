import { Component, OnInit } from '@angular/core';

interface Nav {
  link: string,
  name: string,
  icon: string
}

@Component({
  selector: 'sidenav',
  templateUrl: './sidenav.component.html',
  styleUrls: ['./sidenav.component.scss']
})
export class SidenavComponent implements OnInit {

  constructor() { }

  navs: Nav[] = [
    {
      link: '/',
      name: 'Home',
      icon: 'done'

    },
    {
      link: '/',
      name: 'Speakers',
      icon: 'feedback'
    },
  ]

  ngOnInit() {
  }

}

import { Component, OnInit } from '@angular/core';
import { Router, Event, NavigationStart, RouterEvent } from '@angular/router';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'MITS';
  showHeaderNav: boolean = false;

  constructor(private router: Router) {}

  ngOnInit() {
    this.router.events.subscribe((event) => {
      if (event instanceof NavigationStart) {
        var urlPieces = event.url.split('/');
        if (urlPieces[1] == "admin") {
          this.showHeaderNav = false;
        } else {
          this.showHeaderNav = true;
        }
      }
    });
  }
  
}

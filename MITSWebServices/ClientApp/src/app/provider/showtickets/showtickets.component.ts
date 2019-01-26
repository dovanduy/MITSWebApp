import { Component, OnInit, Input } from "@angular/core";
import { Ticket } from "src/app/core/models";
import { environment } from "../../../environments/environment";

@Component({
  selector: "showtickets",
  templateUrl: "./showtickets.component.html",
  styleUrls: ["./showtickets.component.scss"]
})
export class ShowticketsComponent implements OnInit {
  @Input() tickets: Ticket[];

  constructor() {}

  ngOnInit() {
    console.log(this.tickets);
  }

  openTicket(ticket: Ticket) {
    window.open(`${environment.base_api}tickets/${ticket.qrCode}/ticket.html`);
  }
}

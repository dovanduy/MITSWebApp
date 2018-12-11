import { Component, OnInit } from "@angular/core";
export interface SponsorTable {
  date: number;
  time: number;
  name: string;
  price: number;
}

const SPONSOR_DATA: SponsorTable[] = [
  { date: 1, time: 1700, name: "Tuesday Night Social", price: 300 },
  { date: 2, time: 1900, name: "Wednesday Night Social", price: 300 }
];

@Component({
  selector: "app-sponsors",
  templateUrl: "./sponsors.component.html",
  styleUrls: ["./sponsors.component.scss"]
})
export class SponsorsComponent implements OnInit {
  displayedColumns: string[] = ["date", "time", "name", "price"];
  dataSource = SPONSOR_DATA;

  constructor() {}

  ngOnInit() {}
}

import { Component, OnInit, Input, Output, EventEmitter } from "@angular/core";

import { AllSpeakers } from "../../../graphql/generated/graphql";
import { AdminDataService } from "../../services/admin-data.service";

@Component({
  selector: "speakers-list",
  templateUrl: "./speakers-list.component.html",
  styleUrls: ["./speakers-list.component.scss"]
})
export class SpeakersListComponent implements OnInit {
  constructor(private adminData: AdminDataService) {}

  @Input() speakers: AllSpeakers.Speakers[];
  @Output() onEdit = new EventEmitter<AllSpeakers.Speakers>();
  activeSpeaker: AllSpeakers.Speakers;

  ngOnInit() {
    
    this.adminData.removeActiveFromSpeakerList$.subscribe(value => {
      this.activeSpeaker = null;
    });
  }

  editing(speaker: AllSpeakers.Speakers) {
    this.activeSpeaker = speaker;
    this.onEdit.emit(speaker);
  }
}

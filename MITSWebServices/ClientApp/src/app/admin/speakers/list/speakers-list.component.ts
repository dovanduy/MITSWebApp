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
  @Output() edit = new EventEmitter<AllSpeakers.Speakers>();
  editingSpeaker: AllSpeakers.Speakers;

  ngOnInit() {
    
    this.adminData.removeActiveFromSpeakerList$.subscribe(value => {
      this.editingSpeaker = null;
    });
  }

  editing(speaker: AllSpeakers.Speakers) {
    this.editingSpeaker = speaker;
    this.edit.emit(speaker);
  }
}

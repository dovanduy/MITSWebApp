import { Component, OnInit, Output, EventEmitter } from "@angular/core";
import { AdminDataService } from "../services/admin-data.service";
import {
  FormGroup,
  FormControl,
  FormBuilder,
  Validators
} from "@angular/forms";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";

import { FormField } from "../models/common";
import {
  AllSpeakers,
  AllSpeakersSectionsGQL,
  SpeakerInput,
  CreateSpeakerGQL,
  DeleteSpeakerGQL,
  UpdateSpeakerGQL,
  UpdateSpeaker
} from "../../graphql/generated/graphql";

@Component({
  selector: "app-speakers",
  templateUrl: "./speakers.component.html",
  styleUrls: ["./speakers.component.scss"]
})
export class SpeakersComponent implements OnInit {
  constructor(
    private fb: FormBuilder,
    private adminData: AdminDataService,
    private allSpeakersSectionsGQL: AllSpeakersSectionsGQL,
    private createSpeakerGQL: CreateSpeakerGQL,
    private deleteSpeakerGQL: DeleteSpeakerGQL,
    private updateSpeakerGQL: UpdateSpeakerGQL
  ) {}

  // speakers: Observable<AllSpeakers.Speakers[]>
  speakers: AllSpeakers.Speakers[];
  isOverSpeaker: number;
  editingSpeaker: AllSpeakers.Speakers;
  showAddForm: boolean = false;
  addSpeakerForm: FormGroup;
  addSpeakerFormFields: FormField[];

  ngOnInit() {
    this.adminData.pageTitle("Speakers");

    this.allSpeakersSectionsGQL.watch().valueChanges.subscribe(result => {
      this.speakers = result.data.speakers;
      console.log(this.speakers);
    });

    this.createAddSpeakerStuff();
  }

  getSpeakers(): void {}

  addSpeaker(newSpeakerForm: FormGroup): void {

    console.log(newSpeakerForm);
    var newSpeaker: SpeakerInput =
    {
      firstName: newSpeakerForm.value.firstName,
      lastName: newSpeakerForm.value.lastName,
      title: newSpeakerForm.value.title,
      bio: newSpeakerForm.value.bio

    };


    this.createSpeakerGQL
      .mutate({
        speaker: newSpeaker
      })
      .subscribe(result => {
        this.speakers.push(result.data.createSpeaker);
      });
  }

  deleteSpeaker(speaker: AllSpeakers.Speakers) {
    this.deleteSpeakerGQL
      .mutate({
        speakerId: speaker.id
      })
      .subscribe(result => {
        //Update the speaker list
      });
  }

  editSpeaker(speaker: UpdateSpeaker.UpdateSpeaker) {
    console.log(speaker);
    this.updateSpeakerGQL
      .mutate({
        speaker: speaker
      })
      .subscribe(result => {
        //update the speaker list
      });
  }

  createAddSpeakerStuff() {

    this.addSpeakerForm = this.fb.group({
      firstName: ["", [Validators.required, Validators.max(100)]],
      lastName: ["", [Validators.required, Validators.max(100)]],
      title: ["", [Validators.required, Validators.max(100)]],
      bio: ["", [Validators.required, Validators.max(1000)]]
    });
    

    this.addSpeakerFormFields = [
      {
        name: "First Name",
        formControlName: "firstName"
      },
      {
        name: "Last Name",
        formControlName: "lastName"
      },
      {
        name: "Title",
        formControlName: "title"
      },
      {
        name: "Bio",
        formControlName: "bio"
      }
    ];
  }

  showAddSpeakerForm() {
    this.showAddForm = true;
  }

  closeAddForm() {
    this.showAddForm = false;
  }

  editing(speaker: AllSpeakers.Speakers) {
    console.log("Editing called in the speakers componet.");
    this.editingSpeaker = speaker;
  }

  close(value: boolean): void {
    this.editingSpeaker = null;
  }
}

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
  activeSpeaker: AllSpeakers.Speakers;
  showAddForm: boolean = false;
  showEditForm: boolean = false;
  speakerForm: FormGroup;
  speakerFormFields: FormField[];

  ngOnInit() {
    this.adminData.pageTitle("Speakers");

    this.allSpeakersSectionsGQL.watch().valueChanges.subscribe(result => {
      console.log(result);
      this.speakers = result.data.speakers;
    });
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

  createSpeakerForm() {

    this.speakerForm = this.fb.group({
      firstName: ["", [Validators.required, Validators.max(100)]],
      lastName: ["", [Validators.required, Validators.max(100)]],
      title: ["", [Validators.required, Validators.max(100)]],
      bio: ["", [Validators.required, Validators.max(1000)]]
    });
    

    this.speakerFormFields = [
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

  createEditSpeakerForm(speaker: AllSpeakers.Speakers) {
    this.createSpeakerForm();

    this.speakerForm.setValue({
      firstName: speaker.firstName,
      lastName: speaker.lastName,
      title: speaker.title,
      bio: speaker.bio
    });

  }

  showAddSpeakerForm() {
    this.createSpeakerForm();
    this.showAddForm = true;
  }

  closeAddSpeakerForm() {
    this.showAddForm = false;
  }

  deleteSpeaker(speakerId: number) {
    this.deleteSpeakerGQL
      .mutate({
        speakerId: speakerId
      })
      .subscribe(result => {
        this.speakers = this.speakers.filter(speaker => speaker.id != speakerId);
      });
  }

  editSpeaker(editForm: FormGroup) {
    console.log(editForm);

    var updatedSpeaker: UpdateSpeaker.UpdateSpeaker = 
    {
      id: this.activeSpeaker.id,
      firstName: editForm.value.firstName,
      lastName: editForm.value.lastName,
      title: editForm.value.title,
      bio: editForm.value.bio
    }

    console.log(updatedSpeaker);

    this.updateSpeakerGQL
      .mutate({
        speaker: updatedSpeaker
      })
      .subscribe(result => {
        //update the speaker list
      });
  }

  showEditSpeakerForm(speaker: AllSpeakers.Speakers) {
    this.showEditForm = true;
    this.activeSpeaker = speaker;
    this.createEditSpeakerForm(speaker);
  }

  closeEditSpeakerForm(value: boolean) {
    this.showEditForm = false;
    this.adminData.removeActiveFromSpeakerList(true);
  }

  resetEditSpeakerForm(speaker: AllSpeakers.Speakers) {
    this.showAddForm = false;
    this.speakerForm.reset({
      firstName: speaker.firstName,
      lastName: speaker.lastName,
      title: speaker.title,
      bio: speaker.bio
    });
  }



}

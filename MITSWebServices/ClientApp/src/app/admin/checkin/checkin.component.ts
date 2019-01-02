import { Component, OnInit, ViewChild } from '@angular/core';

import { AdminDataService } from '../services/admin-data.service';
import { ZXingScannerComponent } from '@zxing/ngx-scanner';

import { Result } from '@zxing/library';

@Component({
  selector: 'app-checkin',
  templateUrl: './checkin.component.html',
  styleUrls: ['./checkin.component.scss']
})
export class CheckinComponent implements OnInit {

  constructor(private adminData: AdminDataService) { }

  @ViewChild('scanner')
  scanner: ZXingScannerComponent;
  
  hasDevices: boolean;
  hasPermission: boolean;
  qrResultString: string;
  qrResult: Result;

  availableDevices: MediaDeviceInfo[];
  currentDevice: MediaDeviceInfo;

  ngOnInit() {
    this.adminData.pageTitle('Check In');

    this.scanner.camerasFound.subscribe((devices: MediaDeviceInfo[]) => {
      this.hasDevices = true;
      this.availableDevices = devices;
      this.scanner.changeDevice(devices[0]);
      this.currentDevice = devices[0];
    });

    this.scanner.camerasNotFound.subscribe(() => this.hasDevices = false);
    this.scanner.scanComplete.subscribe((result: Result) => this.qrResult = result);
    this.scanner.permissionResponse.subscribe((perm: boolean) => this.hasPermission = perm);
  }

  handleQrCodeResult(resultString: string) {
    console.debug('Result: ', resultString);
    this.qrResultString = resultString;
  }

  displayCameras(cameras: MediaDeviceInfo[]) {
    console.debug('Devices: ', cameras);
    this.availableDevices = cameras;
  }

  onDeviceSelectChange(selectedValue: string) {
    console.debug('Selection changed: ', selectedValue);
    this.currentDevice = this.scanner.getDeviceById(selectedValue);
  }

  stateToEmoji(state: boolean): string {

    const states = {
      // not checked
      undefined: '❔',
      // failed to check
      null: '⭕',
      // success
      true: '✔',
      // can't touch that
      false: '❌'
    };

    return states['' + state];
  }


}

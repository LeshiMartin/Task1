import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject, Subject } from 'rxjs';
import { filter } from 'rxjs/operators';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class HubService {
  constructor() {
    this.hub = this.getHubConnection();
    this.startConnection(this.hub);
    this.connectionState$
      .pipe(filter((x) => x === signalR.HubConnectionState.Connected))
      .subscribe((x) => {
        this.on('fileIsInProcess', (id) => {
          console.log('file is in proces');
          this.onFileIsProcessing.next(id);
        });
        this.on('fileIsNotValid', (id) => {
          console.log('file is not valid');
          this.onFileIsNoValidChange.next(id);
        });
        this.on('fileIsProcessed', (id) => {
          console.log('file is processed');
          this.onFileIsProcessed.next(id);
        });
      });
  }
  private hub: signalR.HubConnection;
  private getHubConnection(): signalR.HubConnection {
    return new signalR.HubConnectionBuilder()
      .withUrl(`${environment.apiServer}/hub`, {
        logger: signalR.LogLevel.Critical,
        transport: signalR.HttpTransportType.WebSockets,
      })
      .withAutomaticReconnect([
        1000,
        2000,
        3000,
        4000,
        8000,
        10000,
        10000,
        16000,
        30000,
        45000,
      ])
      .build();
  }

  private startConnection(hub: signalR.HubConnection) {
    hub.start().then((x) => this.connectionState$.next(hub.state));
    hub.onreconnected(() => this.connectionState$.next(hub.state));
  }

  private on(method: functionCalls, callback: (...args: any) => void) {
    this.hub.on(method, (args) => callback(args));
  }

  private onFileIsNoValidChange = new Subject<number>();
  private onFileIsProcessing = new Subject<number>();
  private onFileIsProcessed = new Subject<number>();

  private connectionState$ = new BehaviorSubject<signalR.HubConnectionState>(
    signalR.HubConnectionState.Disconnected
  );
  fileIsNotValid$ = this.onFileIsNoValidChange.asObservable();
  fileIsProcessing$ = this.onFileIsProcessing.asObservable();
  fileIsProcessed$ = this.onFileIsProcessed.asObservable();
}

type functionCalls = 'fileIsInProcess' | 'fileIsNotValid' | 'fileIsProcessed';

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AvatarService {
  private apiUrl = '/api/avatar';

  constructor(private http: HttpClient) { }

  getLiveAvatarConfig(): Observable<any> {
    return this.http.get(`${this.apiUrl}/live-config`);
  }

  processVoiceCommand(audioFile: File): Observable<any> {
    const formData = new FormData();
    formData.append('audioFile', audioFile);
    return this.http.post(`${this.apiUrl}/voice-command`, formData);
  }

  synthesizeSpeech(text: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/synthesize`, { text });
  }

  healthCheck(): Observable<any> {
    return this.http.get(`${this.apiUrl}/health`);
  }
}

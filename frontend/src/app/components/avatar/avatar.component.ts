import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AvatarService } from '../../services/avatar.service';

@Component({
  selector: 'app-avatar',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './avatar.component.html',
  styleUrls: ['./avatar.component.css']
})
export class AvatarComponent implements OnInit {
  isListening = false;
  lastCommand = '';
  avatarResponse = '';
  loading = false;
  textToSpeak = '';
  isRecording = false;
  mediaRecorder: MediaRecorder | null = null;
  audioChunks: Blob[] = [];

  constructor(private avatarService: AvatarService) { }

  ngOnInit(): void {
    this.checkAvatarService();
  }

  checkAvatarService(): void {
    this.avatarService.healthCheck().subscribe({
      next: (response) => {
        console.log('Avatar service is running:', response);
      },
      error: (err) => {
        console.error('Avatar service error:', err);
      }
    });
  }

  async startListening(): Promise<void> {
    try {
      this.isRecording = true;
      const stream = await navigator.mediaDevices.getUserMedia({ audio: true });
      this.mediaRecorder = new MediaRecorder(stream);
      this.audioChunks = [];

      this.mediaRecorder.ondataavailable = (event: BlobEvent) => {
        this.audioChunks.push(event.data);
      };

      this.mediaRecorder.onstop = () => {
        const audioBlob = new Blob(this.audioChunks, { type: 'audio/wav' });
        const audioFile = new File([audioBlob], 'voice-command.wav', { type: 'audio/wav' });
        this.processVoiceCommand(audioFile);
        this.isRecording = false;
      };

      this.mediaRecorder.start();
      this.isListening = true;
    } catch (error) {
      console.error('Error accessing microphone:', error);
      this.isRecording = false;
    }
  }

  stopListening(): void {
    if (this.mediaRecorder) {
      this.mediaRecorder.stop();
    }
    this.isListening = false;
  }

  processVoiceCommand(audioFile: File): void {
    this.loading = true;
    this.avatarService.processVoiceCommand(audioFile).subscribe({
      next: (response) => {
        this.lastCommand = response.command;
        this.avatarResponse = `Command received: ${response.command}`;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error processing voice command:', err);
        this.avatarResponse = 'Error processing voice command. Please try again.';
        this.loading = false;
      }
    });
  }

  synthesizeSpeech(): void {
    if (!this.textToSpeak) {
      alert('Please enter text to synthesize');
      return;
    }
    this.loading = true;
    this.avatarService.synthesizeSpeech(this.textToSpeak).subscribe({
      next: (response) => {
        this.avatarResponse = response.result;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error synthesizing speech:', err);
        this.avatarResponse = 'Error synthesizing speech. Please try again.';
        this.loading = false;
      }
    });
  }
}

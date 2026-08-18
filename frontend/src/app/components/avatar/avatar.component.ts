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
  liveAvatarName = 'Lisa';
  liveAvatarStatus = 'Initializing Live Azure Avatar';
  liveAvatarReady = false;
  liveAvatarUrl = '';
  statusMessage = '';

  constructor(private avatarService: AvatarService) { }

  ngOnInit(): void {
    this.checkAvatarService();
    this.loadLiveAvatarConfig();
  }

  loadLiveAvatarConfig(): void {
    this.avatarService.getLiveAvatarConfig().subscribe({
      next: (response) => {
        this.liveAvatarName = response.avatarName || 'Lisa';
        this.liveAvatarReady = !!response.enabled;
        this.liveAvatarUrl = response.liveAvatarVideoUrl || '';
        this.statusMessage = response.message || 'Azure Live Avatar is not configured yet.';
        this.liveAvatarStatus = this.liveAvatarReady
          ? `${this.liveAvatarName} is live and ready`
          : 'Waiting for Azure Speech Live Avatar configuration';
      },
      error: (err) => {
        console.error('Unable to load live avatar configuration:', err);
        this.liveAvatarStatus = 'Live Avatar configuration unavailable';
        this.statusMessage = 'Add Azure Speech Live Avatar settings to enable the live human avatar.';
      }
    });
  }

  checkAvatarService(): void {
    this.avatarService.healthCheck().subscribe({
      next: (response) => {
        console.log('Avatar service is running:', response);
        this.statusMessage = response.message || this.statusMessage;
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
      const mimeType = MediaRecorder.isTypeSupported('audio/webm') ? 'audio/webm' : 'audio/mp4';
      this.mediaRecorder = new MediaRecorder(stream, { mimeType });
      this.audioChunks = [];

      this.mediaRecorder.ondataavailable = (event: BlobEvent) => {
        this.audioChunks.push(event.data);
      };

      this.mediaRecorder.onstop = async () => {
        const audioBlob = new Blob(this.audioChunks, { type: mimeType });
        const wavBlob = await this.convertBlobToWav(audioBlob);
        const audioFile = new File([wavBlob], 'voice-command.wav', { type: 'audio/wav' });
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

  private async convertBlobToWav(blob: Blob): Promise<Blob> {
    const arrayBuffer = await blob.arrayBuffer();
    const audioContext = new AudioContext();
    const audioBuffer = await audioContext.decodeAudioData(arrayBuffer.slice(0));
    const wavBuffer = this.audioBufferToWav(audioBuffer);
    audioContext.close();
    return new Blob([wavBuffer], { type: 'audio/wav' });
  }

  private audioBufferToWav(audioBuffer: AudioBuffer): ArrayBuffer {
    const numberOfChannels = audioBuffer.numberOfChannels;
    const length = audioBuffer.length * numberOfChannels * 2 + 44;
    const buffer = new ArrayBuffer(length);
    const view = new DataView(buffer);

    const channels: Float32Array[] = [];
    for (let i = 0; i < numberOfChannels; i++) {
      channels.push(audioBuffer.getChannelData(i));
    }

    this.writeString(view, 0, 'RIFF');
    view.setUint32(4, 36 + audioBuffer.length * numberOfChannels * 2, true);
    this.writeString(view, 8, 'WAVE');
    this.writeString(view, 12, 'fmt ');
    view.setUint32(16, 16, true);
    view.setUint16(20, 1, true);
    view.setUint16(22, numberOfChannels, true);
    view.setUint32(24, audioBuffer.sampleRate, true);
    view.setUint32(28, audioBuffer.sampleRate * numberOfChannels * 2, true);
    view.setUint16(32, numberOfChannels * 2, true);
    view.setUint16(34, 16, true);
    this.writeString(view, 36, 'data');
    view.setUint32(40, audioBuffer.length * numberOfChannels * 2, true);

    let offset = 44;
    for (let i = 0; i < audioBuffer.length; i++) {
      for (let channel = 0; channel < numberOfChannels; channel++) {
        const sample = Math.max(-1, Math.min(1, channels[channel][i]));
        view.setInt16(offset, sample < 0 ? sample * 0x8000 : sample * 0x7fff, true);
        offset += 2;
      }
    }

    return buffer;
  }

  private writeString(view: DataView, offset: number, text: string): void {
    for (let i = 0; i < text.length; i++) {
      view.setUint8(offset + i, text.charCodeAt(i));
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

using SpotifyNet.Core.Utilities;
using SpotifyNet.Datastructures.Spotify.Tracks;
using SpotifyNet.Datastructures.Spotify.Tracks.Analysis;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Repositories.WebAPI;

public partial class WebAPIRepository
{
    public Task<Track> GetTrack(
        string trackId,
        string accessToken,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(trackId, nameof(trackId));

        var uri = Endpoints.GetTrack(trackId);

        return _webAPIClient.GetAsync<Track>(
            uri,
            accessToken,
            cancellationToken);
    }

    public async Task<IEnumerable<Track>> GetTracks(
        string[] trackIds,
        string accessToken,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(trackIds, nameof(trackIds));

        Ensure.Between(trackIds.Length, 0, 50, inclusive: true);

        var uri = Endpoints.GetSeveralTracks(trackIds);

        var tracks = await _webAPIClient.GetAsync<TracksSet>(
            uri,
            accessToken,
            cancellationToken);

        return tracks.Tracks;
    }

    public Task<IEnumerable<SavedTrack>> GetCurrentUserSavedTracks(
        string accessToken,
        CancellationToken cancellationToken)
    {
        var uri = Endpoints.GetUserSavedTracks();

        return GetPaginated<SavedTrack>(
            uri,
            accessToken,
            cancellationToken);
    }

    public async Task<IEnumerable<AudioFeatures>> GetTracksAudioFeatures(
        string[] trackIds,
        string accessToken,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(trackIds, nameof(trackIds));

        Ensure.Between(trackIds.Length, 0, 100, inclusive: true);

        var uri = Endpoints.GetTracksAudioFeatures(trackIds);

        var features = await _webAPIClient.GetAsync<AudioFeaturesSet>(
            uri,
            accessToken,
            cancellationToken);

        return features.AudioFeatures;
    }

    public async Task<AudioFeatures> GetTrackAudioFeatures(
        string trackId,
        string accessToken,
        CancellationToken cancellationToken)
    {
        var uri = Endpoints.GetTrackAudioFeatures(trackId);

        var features = await _webAPIClient.GetAsync<AudioFeatures>(
            uri,
            accessToken,
            cancellationToken);

        return features;
    }

    public async Task<AudioAnalysis> GetAudioAnalysis(
        string trackId,
        string accessToken,
        CancellationToken cancellationToken)
    {
        var uri = Endpoints.GetTrackAudioAnalysis(trackId);

        return await _webAPIClient.GetAsync<AudioAnalysis>(
            uri,
            accessToken,
            cancellationToken);
    }
}

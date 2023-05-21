using SpotifyNet.Core.Utilities;
using SpotifyNet.Datastructures.Spotify.Tracks;
using SpotifyNet.Datastructures.Spotify.Tracks.Analysis;
using System;
using System.Collections.Generic;
using System.Linq;
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

    public async Task<IReadOnlyList<Track>> GetTracks(
        IEnumerable<string> trackIds,
        string accessToken,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(trackIds, nameof(trackIds));

        var trackIdsCollection = trackIds as ICollection<string> ?? trackIds.ToList();

        Ensure.Between(trackIdsCollection.Count, 1, 50, inclusive: true);
        var uri = Endpoints.GetSeveralTracks(trackIdsCollection);

        var tracks = await _webAPIClient.GetAsync<TracksSet>(
            uri,
            accessToken,
            cancellationToken);

        return tracks.Tracks;
    }

    public Task<IReadOnlyList<SavedTrack>> GetCurrentUserSavedTracks(
        string accessToken,
        CancellationToken cancellationToken)
    {
        var uri = Endpoints.GetUserSavedTracks();

        return GetOffsetPaginated<SavedTrack>(
            uri,
            accessToken,
            cancellationToken);
    }

    public async Task<IReadOnlyList<AudioFeatures>> GetTracksAudioFeatures(
        IEnumerable<string> trackIds,
        string accessToken,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(trackIds, nameof(trackIds));

        var trackIdsCollection = trackIds as ICollection<string> ?? trackIds.ToList();
        Ensure.Between(trackIdsCollection.Count, 1, 100, inclusive: true);

        var uri = Endpoints.GetTracksAudioFeatures(trackIdsCollection);

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

    public Task<IReadOnlyList<bool>> AreTracksSaved(
        IEnumerable<string> trackIds,
        string accessToken,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(trackIds, nameof(trackIds));

        var trackIdsCollection = trackIds as ICollection<string> ?? trackIds.ToList();
        Ensure.Between(trackIdsCollection.Count, 1, 50, inclusive: true);

        var uri = Endpoints.CheckUserSavedTracks(trackIdsCollection);

        return _webAPIClient.GetAsync<IReadOnlyList<bool>>(
            uri,
            accessToken,
            cancellationToken);
    }
}

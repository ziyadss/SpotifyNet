using SpotifyNet.Core.Utilities;
using SpotifyNet.Datastructures.Spotify.Tracks;
using SpotifyNet.Datastructures.Spotify.Tracks.Analysis;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SpotifyNet.Repositories.WebAPI;

public partial class WebAPIRepository
{
    public async Task<IEnumerable<Track>> GetTracks(
        string[] trackIds,
        string accessToken,
        CancellationToken cancellationToken)
    {
        Ensure.Between(trackIds.Length, 0, 50, inclusive: true);

        var url = Endpoints.GetSeveralTracks(trackIds);

        var tracks = await _webAPIClient.GetAsync<TracksSet>(
            url,
            accessToken,
            cancellationToken);

        return tracks.Tracks;
    }

    public Task<IEnumerable<SavedTrack>> GetCurrentUserSavedTracks(
        string accessToken,
        CancellationToken cancellationToken)
    {
        var url = Endpoints.GetUserSavedTracks();

        return GetPaginated<SavedTrack>(
            url,
            accessToken,
            cancellationToken);
    }

    public async Task<IEnumerable<AudioFeatures>> GetTracksAudioFeatures(
        string[] trackIds,
        string accessToken,
        CancellationToken cancellationToken)
    {
        Ensure.Between(trackIds.Length, 0, 100, inclusive: true);

        var url = Endpoints.GetTracksAudioFeatures(trackIds);

        var features = await _webAPIClient.GetAsync<AudioFeaturesSet>(
            url,
            accessToken,
            cancellationToken);

        return features.AudioFeatures;
    }

    public async Task<AudioFeatures> GetTrackAudioFeatures(
        string trackId,
        string accessToken,
        CancellationToken cancellationToken)
    {
        var url = Endpoints.GetTrackAudioFeatures(trackId);

        var features = await _webAPIClient.GetAsync<AudioFeatures>(
            url,
            accessToken,
            cancellationToken);

        return features;
    }

    public async Task<AudioAnalysis> GetAudioAnalysis(
        string trackId,
        string accessToken,
        CancellationToken cancellationToken)
    {
        var url = Endpoints.GetTrackAudioAnalysis(trackId);

        return await _webAPIClient.GetAsync<AudioAnalysis>(
            url,
            accessToken,
            cancellationToken);
    }
}
